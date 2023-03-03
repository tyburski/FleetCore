namespace FleetCore.Models
{
    public class Leave
    {
        public int Id { get; set; }

        public AppUser User { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LeaveDate { get; set; }

    }
}
