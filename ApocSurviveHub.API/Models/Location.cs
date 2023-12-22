namespace ApocSurviveHub.API.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Coordinates Coordinates { get; set; }

        private Location()
        {
            Coordinates = new Coordinates();
        }

        public Location(string name, double longitude, double latitude) : this()
        {
            Coordinates.Longitude = longitude;
            Coordinates.Latitude = latitude;
            Name = name;
        }
    }
}
