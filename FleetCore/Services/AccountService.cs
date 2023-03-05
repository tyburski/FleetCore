using FleetCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FleetCore.Services
{
    public interface IAccountService
    {
        Task<bool> Create(CreateUserModel model);
    }
    public class AccountService : IAccountService
    {
        private readonly FleetCoreDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(FleetCoreDbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Create(CreateUserModel model)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = _dbContext.Users.Include(x => x.Organization).FirstOrDefault(x => x.Id.Equals(userId));

            string username = ($"{model.FirstName.Substring(0, 1)}{model.LastName}").ToLower();
            var dbCheck = _dbContext
                .Users
                .FirstOrDefault(x=>x.UserName== username);

            bool registerSuccess = false;
            if (dbCheck is null && currentUser is not null)
            {
                var user = new AppUser
                {
                    UserName = username,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    FullName = $"{model.FirstName} {model.LastName}",
                    Role = "User",
                    Organization = currentUser.Organization,
                    Password = currentUser.Organization.OrganizationPassword
                };

                registerSuccess = true;
            }
            return registerSuccess;
        }
        
    }
}
