
namespace ActionApp.Models
{
    public class DirectorItem
    {
        public string NameId { get; set; } = string.Empty;
        public string? Name { get; set; }
        public int? BirthYear { get; set; }
        public int MovieCount { get; set; }
        public string? TopMovie { get; set; }
    }
}
