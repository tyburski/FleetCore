using FleetCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace FleetCore.Services
{
    public interface IVehicleService
    {
        Vehicle GetByPlate(string plate);
        IEnumerable<Vehicle> GetAll();
        Task<bool> Create(CreateVehicleModel model);
        int Update(int id, UpdateVehicleModel model);
        bool Delete(int id);
        Task<bool> CreateRepair(CreateRepairModel model);
        Task<bool> FinishRepair(FinishRepairModel model);
        Task<bool> CreateRefueling(CreateRefuelingModel model);
        Task<bool> UpdateEvent(UpdateEventDate model);
    }
    public class VehicleService : IVehicleService
    {
        private readonly FleetCoreDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VehicleService(FleetCoreDbContext dbContext,
                                IHttpContextAccessor httpContextAccessor) 
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public Vehicle GetByPlate(string plate)
        {
            var vehicle = _dbContext
                .Vehicles
                .Include(x => x.Events)
                .Include(x => x.Repairs).ThenInclude(x=>x.User)
                .Include(x => x.Repairs).ThenInclude(x => x.UserFinished)
                .Include(x=>x.Refuelings)
                .FirstOrDefault(x => x.Plate == plate);
                

            if (vehicle is null) return null;

            return vehicle;
        }
        public IEnumerable<Vehicle> GetAll()
        {         
            var vehicles = _dbContext
                .Vehicles
                .Include(x => x.Events)
                .ToList();
            if (vehicles is null) return Enumerable.Empty<Vehicle>();
            return vehicles;
        }
        public async Task<bool> Create(CreateVehicleModel model)
        {
            if (_dbContext.Vehicles.FirstOrDefault(x => x.Plate == model.Plate) is not null) return false;
            Vehicle vehicle = new Vehicle()
            {
                Plate = model.Plate,
                Mileage = model.Mileage,
                VIN = model.VIN,
            };
            _dbContext.Vehicles.Add(vehicle);

            Event event1 = new Event()
            {
                Content = "Ubezpieczenie OC",
                Vehicle= vehicle,
                Date= DateTime.Now
            };
            Event event2 = new Event()
            {
                Content = "Przegląd Techniczny",
                Vehicle = vehicle,
                Date = DateTime.Now
            };
            Event event3 = new Event()
            {
                Content = "Przegląd Windy",
                Vehicle = vehicle,
                Date = DateTime.Now
            };
            Event event4 = new Event()
            {
                Content = "Legalizacja Tachografu",
                Vehicle = vehicle,
                Date = DateTime.Now
            };

            _dbContext.Events.Add(event1);
            _dbContext.Events.Add(event2);
            _dbContext.Events.Add(event3);
            _dbContext.Events.Add(event4);

            _dbContext.SaveChanges();
            return true;
        }    
        public int Update(int id, UpdateVehicleModel model)
        {
            var vehicle = _dbContext
                .Vehicles
                .FirstOrDefault(x => x.Id == id);
            
            vehicle.Plate = model.Plate;
            vehicle.Mileage = model.Mileage;
            vehicle.VIN = model.VIN;

            _dbContext.SaveChanges();
            return vehicle.Id;
        }
        public bool Delete(int id)
        {
            var vehicle = _dbContext
                .Vehicles
                .FirstOrDefault(x => x.Id == id);
            if (vehicle is null) return false;

            _dbContext.Vehicles.Remove(vehicle);
            _dbContext.SaveChanges();
            return true;
        }

        public async Task<bool> CreateRepair(CreateRepairModel model)
        {
            var vehicle = _dbContext
                .Vehicles
                .FirstOrDefault(x => x.Plate.Equals(model.Plate));
            var user = _dbContext.Users.FirstOrDefault(x => x.Id.ToString().Equals(model.userId));
            if (vehicle is null || user is null) return false;

            var repair = new Repair()
            {
                Content = model.Content,
                Vehicle = vehicle,
                User = user,
                CreatedAt = DateTime.Now,
                FinishAt = DateTime.Now
            };

            _dbContext.Repairs.Add(repair);
            _dbContext.SaveChanges();

            return true;
        }
        public async Task<bool> FinishRepair(FinishRepairModel model)
        {
            var repair = _dbContext.Repairs.Include(x=>x.Vehicle).FirstOrDefault(x => x.Id.Equals(model.repairId));
            var user = _dbContext.Users.Include(x=>x.Bonuses).FirstOrDefault(x=>x.Id.ToString().Equals(model.userId));
            if (repair is null || user is null) return false;
            else
            {
                if(model.isBonus is false)
                {
                    repair.UserFinished = user;
                    repair.FinishAt = DateTime.Now;
                    _dbContext.SaveChanges();
                    return true;
                }
                else
                {
                    var bonus = new Bonus()
                    {
                        Content = $"Zakończenie naprawy: [{repair.Content}] Pojazd: [{repair.Vehicle.Plate}]",
                        User = user,
                        CreatedAt = DateTime.Now
                    };
                    _dbContext.Bonuses.Add(bonus);
                    repair.UserFinished = user;
                    repair.FinishAt = DateTime.Now;
                    _dbContext.SaveChanges();
                    return true;
                }
                
            }
        }
        public async Task<bool> CreateRefueling(CreateRefuelingModel model)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id.ToString().Equals(model.userId));


            var vehicle = _dbContext
                .Vehicles
                .FirstOrDefault(x => x.Plate == model.Plate);

            if (vehicle is null || user is null) return false;
            if (model.Mileage < vehicle.Mileage) return false;
            var refueling = new Refueling()
            {
                Vehicle = vehicle,
                User = user,
                CreatedAt = DateTime.Now,
                Mileage = model.Mileage,
                Quantity = model.Quantity
            };
            vehicle.Mileage= model.Mileage;
            _dbContext.Update(vehicle);
            _dbContext.Refuelings.Add(refueling);
            _dbContext.SaveChanges();

            return true;
        }

        public async Task<bool> UpdateEvent(UpdateEventDate model)
        {
            var ev = _dbContext.Events.FirstOrDefault(x=>x.Id.Equals(model.Id));
            if(ev is null) return false;
            else
            {
                var dateConverted = DateTime.Parse(model.Date);
                ev.Date = dateConverted;
                _dbContext.SaveChanges();
                return true;
            }
        }
    }
}
