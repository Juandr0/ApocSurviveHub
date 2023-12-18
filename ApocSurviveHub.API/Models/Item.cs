namespace ApocSurviveHub.API.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int SurvivorId { get; set; }
        public Survivor? Survivor { get; set; }

        Item(string name, string type, int survivorId)
        {
            Name = name;
            Type = type;
            SurvivorId = survivorId;
        }
    }

}