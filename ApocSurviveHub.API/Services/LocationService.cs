using ApocSurviveHub.API.Models;
using ApocSurviveHub.API.Interfaces;

namespace ApocSurviveHub.API.Services
{
    public class LocationService
    {

        private readonly ICrud<Location> _locationRepository;

        public LocationService(ICrud<Location> locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public Location CreateLocation(
            string name,
            double _longitude,
            double _latitude)
        {
            var location = new Location(name, _longitude, _latitude);
            _locationRepository.Create(location);

            return location;
        }
        public IEnumerable<Location> GetLocations()
        {
            return _locationRepository.GetAll(l => l.Coordinates);
        }

        public Location GetById(int locationId)
        {
            return _locationRepository.GetById(locationId, l => l.Coordinates);
        }
        public Location? UpdateLocation(
            int locationId,
            string? name,
            double? longitude,
            double? latitude)
        {
            var location = _locationRepository.GetById(locationId, l => l.Coordinates);
            if (location is null) return null;

            location.Name = name ?? location.Name;

            if (longitude.HasValue && latitude.HasValue)
            {
                location.Coordinates.Longitude = (double)longitude;
                location.Coordinates.Latitude = (double)latitude;
            }

            _locationRepository.Update(location);
            return location;
        }

        public Location DeleteLocation(int locationId)
        {
            var location = _locationRepository.GetById(locationId);
            _locationRepository.Delete(location);
            return location;
        }

    }
}
