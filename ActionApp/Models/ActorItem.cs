namespace ActionApp.Models
{
    public class ActorItem
    {
        public string NameId { get; set; } = string.Empty;
        public string? PrimaryName { get; set; }
        public int? BirthYear { get; set; }
        public int MovieCount { get; set; }
        public string? TopMovie { get; set; }
    }
}
