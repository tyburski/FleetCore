using FleetCore.Models;
using Microsoft.AspNetCore.Identity;

namespace FleetCore
{
    public static class DataSeeder
    {
        public static async void Initialize(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = (UserManager<AppUser>)scope.ServiceProvider.GetService(typeof(UserManager<AppUser>));
                var roleManager = (RoleManager<IdentityRole>)scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>));

                if (await roleManager.RoleExistsAsync("User") == false)
                {
                    IdentityRole newRole = new IdentityRole("User");
                    await roleManager.CreateAsync(newRole);
                }
                if (await roleManager.RoleExistsAsync("Moderator") == false)
                {
                    IdentityRole newRole = new IdentityRole("Moderator");
                    await roleManager.CreateAsync(newRole);
                }
                if (await roleManager.RoleExistsAsync("Owner") == false)
                {
                    IdentityRole newRole = new IdentityRole("Owner");
                    await roleManager.CreateAsync(newRole);
                }
                if (await roleManager.RoleExistsAsync("Admin") == false)
                {
                    IdentityRole newRole = new IdentityRole("Admin");
                    await roleManager.CreateAsync(newRole);
                }
            }
        }
    }
}
