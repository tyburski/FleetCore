using FleetCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FleetCore.Services
{
    public interface IOrganizationService
    {
        bool Create(CreateOrganizationModel model);
        IEnumerable<Organization> GetAll();
        IEnumerable<Organization> GetByName(string search);
        Organization GetById(string id);
        Guid Update(string id, UpdateOrganizationModel model);
        bool Delete(string id);
    }
    public class OrganizationService : IOrganizationService
    {
        private readonly FleetCoreDbContext _dbContext;

        public OrganizationService(FleetCoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Create(CreateOrganizationModel model)
        {
            string username = ($"{model.FirstName.Substring(0, 1)}{model.LastName}").ToLower();
            var orgDbCheck = _dbContext
                .Organizations
                .FirstOrDefault(x => x.Name == model.Name);
            var userDbCheck = _dbContext
                .Users
                .FirstOrDefault(x => x.UserName.Equals(username));

            bool createSuccess = false;
            if (userDbCheck is null && orgDbCheck is null)
            {
                var organization = new Organization()
                {
                    Name = model.Name,
                    OrganizationPassword = model.OrganizationPassword
                };
                _dbContext.Organizations.Add(organization);
                var user = new AppUser
                {
                    UserName = username,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    FullName = $"{model.FirstName} {model.LastName}",
                    Organization = organization,
                    Password = organization.OrganizationPassword,
                    Role = "Owner"
                };
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                createSuccess = true;
            }
            return createSuccess;                    
        }
        public IEnumerable<Organization> GetAll()
        {
            var organizations = _dbContext
                .Organizations
                .Include(x => x.Users)
                .Include(x => x.Vehicles)
                .ToList();

            return organizations;
        }
        public IEnumerable<Organization> GetByName(string search)
        {
            var organizations = _dbContext
                .Organizations
                .Include(x => x.Users)
                .Include(x => x.Vehicles)
                .Where(x=>x.Name.Contains(search))
                .ToList();
            return organizations;
        }
        public Organization GetById(string id)
        {
            var organization = _dbContext
                .Organizations
                .Include(x => x.Users)
                .Include(x => x.Vehicles)
                .FirstOrDefault(x => x.Id.Equals(id));
            return organization;
        }
        public Guid Update(string id, UpdateOrganizationModel model)
        {
            var organization = _dbContext
                .Organizations
                .FirstOrDefault(x=>x.Id.Equals(id));
            organization.Name = model.Name;
            organization.OrganizationPassword = model.OrganizationPassword;

            return organization.Id;
        }
        public bool Delete(string id)
        {
            var organization = _dbContext
                .Organizations
                .FirstOrDefault(x => x.Id.Equals(id));
            if (organization == null) return false;

            _dbContext.Organizations.Remove(organization);
            _dbContext.SaveChanges();
            return true;
        }
    }

}
