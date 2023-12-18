namespace ApocSurviveHub.API.Models
{
    public class Survivor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAlive { get; set; }
        public int CurrentLocationId { get; set; }
        public Location? CurrentLocation { get; set; }
        public ICollection<Item> Inventory { get; set; } = new List<Item>();

        public Survivor(String name, bool isAlive, int currentLocationId)
        {
            Name = name;
            IsAlive = isAlive;
            CurrentLocationId = currentLocationId;
        }
    }

}