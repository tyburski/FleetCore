using FleetCore.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FleetCore
{
    public static class DataSeeder
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using (var _dbContext = new FleetCoreDbContext())
            {

                if (!_dbContext.Organizations.Any())
                {
                    Organization organization = new Organization()
                    {
                        Name = "PHU TRANSPORT",
                        Address1 = "ul.Zwierzyniecka 127",
                        Address2 = "31-132 Kraków",
                        NIP = "1060000062"
                    };
                    AppUser user = new AppUser
                    {
                        UserName = "dtyburski",
                        FirstName = "Dawid",
                        LastName = "Tyburski",
                        FullName = "Dawid Tyburski",
                        Role = "Owner",
                        Password = BCrypt.Net.BCrypt.HashPassword("FleetCore")
                    };
                    _dbContext.Organizations.Add(organization);
                    _dbContext.Users.Add(user);
                    _dbContext.SaveChanges();
                }

            }
        }
}
    }
