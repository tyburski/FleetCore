using FleetCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FleetCore.Services
{
    public interface IOrganizationService
    {
        Task<bool>Update(UpdateOrganizationModel model);
        Organization Get();
    }
    public class OrganizationService : IOrganizationService
    {
        private readonly FleetCoreDbContext _dbContext;

        public OrganizationService(FleetCoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Update(UpdateOrganizationModel model)
        {
            var organization = _dbContext
                .Organizations
                .FirstOrDefault();
            if(organization is not null)
            {
                organization.Name = model.Name;
                organization.Address1 = model.Address1;
                organization.Address2 = model.Address2;
                organization.NIP = model.NIP;

               _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
        public Organization Get()
        {
            return _dbContext.Organizations.First();
        }
    }

}
