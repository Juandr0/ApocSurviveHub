namespace ApocSurviveHub.API.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }


        public Item(string name, string type)
        {
            Name = name;
            Type = type;
        
        }
    }

}