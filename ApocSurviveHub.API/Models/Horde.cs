namespace ApocSurviveHub.API.Models
{
    public class Horde
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? LocationId { get; set; }
        public int ThreatLevel { get; set; }

        public Horde(string name, int threatLevel, int? locationId)
        {
            Name = name;
            ThreatLevel = threatLevel;
            LocationId = locationId;
        }
    }
}