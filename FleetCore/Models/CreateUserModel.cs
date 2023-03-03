namespace FleetCore.Models
{
    public class CreateUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; } = "User";
    }
}
