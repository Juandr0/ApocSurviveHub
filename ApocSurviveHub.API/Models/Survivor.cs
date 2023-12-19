namespace ApocSurviveHub.API.Models
{
    public class Survivor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAlive { get; set; }
        public int LocationId { get; set; }
        public ICollection<Item> Inventory { get; set; } = new List<Item>();

        public Survivor(String name, bool isAlive, int locationId)
        {
            Name = name;
            IsAlive = isAlive;
            LocationId = locationId;
        }
    }

}