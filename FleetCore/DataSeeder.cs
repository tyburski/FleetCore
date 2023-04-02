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
                        Name = "PHU PRIMA",
                        Address1 = "Chełm Śląski123 ",
                        Address2 = "41-032 Chełm Śląski",
                        NIP = "565765000"
                    };
                    AppUser user = new AppUser
                    {
                        UserName = "mprzewoznik",
                        FirstName = "Mariusz",
                        LastName = "Przewoźnik",
                        FullName = "Mariusz Przewoźnik",
                        Role = "Owner",
                        Password = BCrypt.Net.BCrypt.HashPassword("Prima123")
                    };
                    _dbContext.Organizations.Add(organization);
                    _dbContext.Users.Add(user);
                    _dbContext.SaveChanges();
                }

            }
        }
}
    }
