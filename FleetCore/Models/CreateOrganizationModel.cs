namespace FleetCore.Models
{
    public class CreateOrganizationModel
    {
        public string Name { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string OrganizationPassword { get; set; }

        public string Role { get; set; } = "Owner";
    }
}
