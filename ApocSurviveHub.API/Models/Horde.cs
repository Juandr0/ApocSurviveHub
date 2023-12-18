namespace ApocSurviveHub.API.Models
{
    public class Horde
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? LastSeenId { get; set; }
        public Location? LastSeen { get; set; }
        public int ThreatLevel { get; set; }

        public Horde(string name, int threatLevel, int? lastSeenId)
        {
            Name = name;
            ThreatLevel = threatLevel;
            LastSeenId = lastSeenId;
        }
    }
}