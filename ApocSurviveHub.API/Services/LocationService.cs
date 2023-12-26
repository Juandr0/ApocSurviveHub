using ApocSurviveHub.API.Models;
using ApocSurviveHub.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult CreateLocation(
            string name,
            double _longitude,
            double _latitude)
        {
            var location = new Location(name, _longitude, _latitude);
            _locationRepository.Create(location);

            return new CreatedResult($"/Location/{location.Id}", location);
        }
        public IEnumerable<Location> GetLocations()
        {
            return _locationRepository.GetAll(l => l.Coordinates);
        }

        public Location GetById(int locationId)
        {
            return _locationRepository.GetById(locationId, l => l.Coordinates);
        }
        public IActionResult UpdateLocation(
            int locationId,
            string? name,
            double? longitude,
            double? latitude)
        {
            var location = _locationRepository.GetById(locationId);
            if (location is null) return new NotFoundResult();

            location.Name = name ?? location.Name;

            if (longitude.HasValue && latitude.HasValue)
            {
                location.Coordinates.Longitude = (double)longitude;
                location.Coordinates.Latitude = (double)latitude;
            }

            _locationRepository.Update(location);
            return new OkObjectResult(location);
        }

    }
}
