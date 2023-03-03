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
        Task<bool> Login(LoginModel model);
        Task<bool> Logout();
    }
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly FleetCoreDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(UserManager<AppUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            SignInManager<AppUser> signInManager,
            FleetCoreDbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Create(CreateUserModel model)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = _dbContext.Users.Include(x => x.Organization).FirstOrDefault(x => x.Id == userId);

            string username = ($"{model.FirstName.Substring(0, 1)}{model.LastName}").ToLower();
            var dbCheck = await _userManager.FindByNameAsync(username);

            bool registerSuccess = false;
            if (dbCheck is null && currentUser is not null)
            {
                var user = new AppUser
                {
                    UserName = username,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    FullName = $"{model.FirstName} {model.LastName}",
                    Organization = currentUser.Organization     
                };
                await _userManager.CreateAsync(user, user.Organization.OrganizationPassword);

                await _userManager.AddClaimAsync(user, new Claim("FullName", user.FullName));

                var role = _roleManager.FindByNameAsync(model.Role).Result;
                await _userManager.AddToRoleAsync(user, role.Name);

                registerSuccess = true;
            }
            return registerSuccess;
        }

        public async Task<bool> Login(LoginModel model)
        {
            var loginSuccess = false;
            var res = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, false);
            if (res.Succeeded)
            {
                loginSuccess = true;
            }
            return loginSuccess;
        }
        
        public async Task<bool> Logout()
        {
            await _signInManager.SignOutAsync();
            return true;
        }
    }
}
