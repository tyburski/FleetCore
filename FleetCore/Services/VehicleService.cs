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
        bool Update(UpdateVehicleModel model);
        bool UpdateMileage(UpdateMileageModel model);
        bool Delete(string plate);
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
                .Include(x=>x.Repairs).ThenInclude(x=>x.UserFinished)
                .ToList();
            if (vehicles is null) return Enumerable.Empty<Vehicle>();
            return vehicles;
        }
        public async Task<bool> Create(CreateVehicleModel model)
        {
            if (_dbContext.Vehicles.FirstOrDefault(x => x.Plate == model.Plate) is not null) return false;
            Vehicle vehicle = new Vehicle()
            {
                Plate = model.Plate.ToUpper(),
                Mileage = model.Mileage,
                VIN = model.VIN.ToUpper(),
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

        public bool Update(UpdateVehicleModel model)
        {
            var vehicle = _dbContext
                .Vehicles
                .FirstOrDefault(x => x.Plate.Equals(model.Plate));
            if (vehicle is null) return false;

            var checkPlate = _dbContext
                .Vehicles
                .FirstOrDefault(x => x.Plate.Equals(model.newPlate));
            if (checkPlate is not null) return false;

            vehicle.Plate = model.newPlate;

            _dbContext.SaveChanges();
            return true;
        }

        public bool UpdateMileage(UpdateMileageModel model)
        {
            var vehicle = _dbContext
                .Vehicles
                .FirstOrDefault(x => x.Plate.Equals(model.Plate));
            if (vehicle is null) return false;

       
            vehicle.Mileage = model.Mileage;

            _dbContext.SaveChanges();
            return true;
        }
        public bool Delete(string plate)
        {
            var vehicle = _dbContext
                .Vehicles
                .FirstOrDefault(x => x.Plate.Equals(plate));
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

            var log = new Log()
            {
                Content = $"[{user.FullName}] dodał naprawę [{repair.Content}] do pojazdu [{repair.Vehicle.Plate}]",
                Date=repair.CreatedAt
            };
            _dbContext.Logs.Add(log);

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

                    var log = new Log()
                    {
                        Content = $"[{user.FullName}] zakończył naprawę [{repair.Content}] w pojeździe [{repair.Vehicle.Plate}] Bonus: NIE",
                        Date = repair.FinishAt
                    };
                    _dbContext.Logs.Add(log);
                    _dbContext.SaveChanges();
                    return true;
                }
                else
                {
                    repair.UserFinished = user;
                    repair.FinishAt = DateTime.Now;

                    var bonus = new Bonus()
                    {
                        Content = $"Zakończenie naprawy: [{repair.Content}] Pojazd: [{repair.Vehicle.Plate}]",
                        User = user,
                        CreatedAt = repair.FinishAt
                    };
                    _dbContext.Bonuses.Add(bonus);
                    

                    var log = new Log()
                    {
                        Content = $"[{user.FullName}] zakończył naprawę [{repair.Content}] w pojeździe [{repair.Vehicle.Plate}] Bonus: TAK",
                        Date = repair.FinishAt
                    };
                    _dbContext.Logs.Add(log);
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

            var log = new Log()
            {
                Content = $"[{user.FullName}] dodał tankowanie [{refueling.Quantity} L] do pojazdu [{refueling.Vehicle.Plate}]",
                Date = refueling.CreatedAt
            };
            _dbContext.Logs.Add(log);
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
