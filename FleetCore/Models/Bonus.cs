namespace FleetCore.Models
{
    public class Bonus
    {
        public int Id { get; set; }

        public AppUser User { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
