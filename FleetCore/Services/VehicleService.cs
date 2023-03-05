using FleetCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FleetCore.Services
{
    public interface IVehicleService
    {
        Vehicle GetById(int id);
        IEnumerable<Vehicle> GetAll();
        int Create(CreateVehicleModel model);
        int Update(int id, UpdateVehicleModel model);
        bool Delete(int id);
        int CreateEvent(int id, CreateEventModel model);
        int CreateRepair(int id, CreateRepairModel model);
        int CreateRefueling(CreateRefuelingModel model);
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
        public Vehicle GetById(int id)
        {
            var vehicle = _dbContext
                .Vehicles
                .Include(x => x.Events)
                .Include(x => x.Repairs)
                .Include(x=>x.Refuelings)
                .FirstOrDefault(x => x.Id == id);
                

            if (vehicle is null) return null;

            return vehicle;
        }
        public IEnumerable<Vehicle> GetAll()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _dbContext.Users.Include(x => x.Organization).FirstOrDefault(x => x.Id.Equals(userId));

            var vehicles = _dbContext
                .Vehicles
                .Include(x => x.Organization)
                .Include(x => x.Events)
                .Include(x => x.Repairs)
                .Where(x => x.Organization == user.Organization)
                .ToList();
            return vehicles;
        }
        public int Create(CreateVehicleModel model)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _dbContext.Users.Include(x=>x.Organization).FirstOrDefault(x=>x.Id.Equals(userId));

            Vehicle vehicle = new Vehicle()
            {
                Plate = model.Plate,
                Mileage = model.Mileage,
                VIN = model.VIN,
                Organization = user.Organization
            };
            _dbContext.Vehicles.Add(vehicle);
            _dbContext.SaveChanges();
            return vehicle.Id;
        }    
        public int Update(int id, UpdateVehicleModel model)
        {
            var vehicle = _dbContext
                .Vehicles
                .FirstOrDefault(x => x.Id == id);
            
            vehicle.Plate = model.Plate;
            vehicle.Mileage = model.Mileage;
            vehicle.VIN = model.VIN;
            vehicle.Note = model.Note;

            _dbContext.SaveChanges();
            return vehicle.Id;
        }
        public bool Delete(int id)
        {
            var vehicle = _dbContext
                .Vehicles
                .FirstOrDefault(x => x.Id == id);
            if(vehicle==null)
            {
                return false;
            }
            _dbContext.Vehicles.Remove(vehicle);
            _dbContext.SaveChanges();
            return true;
        }
        public int CreateEvent(int id, CreateEventModel model)
        {
            var vehicle = _dbContext
                .Vehicles
                .FirstOrDefault(x => x.Id == id);
            var _event = new Event()
            {
                Content = model.Content,
                Vehicle = vehicle,
                Date = model.Date
            };

            _dbContext.Events.Add(_event);
            _dbContext.SaveChanges();

            return vehicle.Id;
        }
        public int CreateRepair(int id, CreateRepairModel model)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _dbContext.Users.Include(x => x.Organization).FirstOrDefault(x => x.Id.Equals(userId));

            var vehicle = _dbContext
                .Vehicles
                .FirstOrDefault(x => x.Id == id);

            var repair = new Repair()
            {
                Content = model.Content,
                Vehicle = vehicle,
                User = user,
                CreatedAt = DateTime.Now               
            };

            _dbContext.Repairs.Add(repair);
            _dbContext.SaveChanges();

            return vehicle.Id;
        }
        public int CreateRefueling(CreateRefuelingModel model)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _dbContext.Users.Include(x => x.Organization).FirstOrDefault(x => x.Id.Equals(userId));

            var vehicle = _dbContext
                .Vehicles
                .FirstOrDefault(x => x.Plate == model.Plate);

            var refueling = new Refueling()
            {
                Vehicle = vehicle,
                User = user,
                CreatedAt = DateTime.Now,
                Mileage = model.Mileage,
                Quantity = model.Quantity
            };

            _dbContext.Refuelings.Add(refueling);
            _dbContext.SaveChanges();

            return vehicle.Id;
        }
    }
}
