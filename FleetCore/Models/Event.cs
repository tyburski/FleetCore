namespace FleetCore.Models
{
    public class Event
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public Vehicle Vehicle { get; set; }

        public DateTime Date { get; set; }
    }
}
