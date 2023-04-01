using Microsoft.AspNetCore.Identity;

namespace FleetCore.Models
{
    public class AppUser
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string Role { get; set; }

        public Vehicle? Vehicle { get; set; }

        public IEnumerable<Bonus>? Bonuses { get; set; }

        public IEnumerable<Refueling>? Refuelings { get; set; }

        public IEnumerable<Repair>? Repairs { get; set; }

        public IEnumerable<Repair>? FinishedRepairs { get; set; }
    }
}
