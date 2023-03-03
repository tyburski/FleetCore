using Microsoft.AspNetCore.Identity;

namespace FleetCore.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public Vehicle? Vehicle { get; set; }

        public IEnumerable<Bonus>? Bonuses { get; set; }

        public IEnumerable<Leave>? Leaves { get; set; }

        public IEnumerable<Refueling>? Refuelings { get; set; }

        public IEnumerable<Repair>? Repairs { get; set; }

        public IEnumerable<Notice>? Notices { get; set; }

        public Organization Organization { get; set; }
    }
}
