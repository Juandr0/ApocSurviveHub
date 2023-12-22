namespace ApocSurviveHub.API.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int? LocationId { get; set; }
        public Location? Location { get; set; }


        public Item(string name, string type, int? locationId)
        {
            Name = name;
            Type = type;
            LocationId = locationId;
        }
    }

}