namespace FleetCore.Models
{
    public class Organization
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string OrganizationPassword { get; set; }

        public IEnumerable<AppUser>? Users { get; set; }

        public IEnumerable<Vehicle>? Vehicles { get; set; }
    }
}
