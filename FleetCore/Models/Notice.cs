namespace FleetCore.Models
{
    public class Notice
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public AppUser? User { get; set; }
    }
}
