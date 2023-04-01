namespace FleetCore.Models
{
    public class Repair
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public AppUser? User { get; set; }

        public Vehicle Vehicle { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime FinishAt { get; set; }

        public AppUser? UserFinished { get; set; }


    }
}
