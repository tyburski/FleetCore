namespace FleetCore.Models
{
    public class Refueling
    {
        public int Id { get; set; }

        public AppUser? User { get; set; }

        public Vehicle Vehicle { get; set; }

        public long Mileage { get; set; }

        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
