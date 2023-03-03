namespace FleetCore.Models
{
    public class SearchBonusModel
    {
        public string FullName { get; set; }
        public int Month { get; set; } = DateTime.Now.Month - 1;
        public int Year { get; set; } = DateTime.Now.Year;
    }
}
