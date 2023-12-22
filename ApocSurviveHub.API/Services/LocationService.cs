using ApocSurviveHub.API.Models;
using ApocSurviveHub.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public abstract class LocationService
    {
        public static async Task<IActionResult> CreateLocation(
            AppDbContext dbContext,
            string name,
            double _longitude,
            double _latitude)
        {
            var location = new Location(name, _longitude, _latitude);
            dbContext.Locations.Add(location);
            await dbContext.SaveChangesAsync();

            return new CreatedResult($"/Location/{location.Id}", location);
        }
        public static async Task<IEnumerable<Location>> GetLocations(AppDbContext dbContext)
        {
            return await dbContext.Locations.Include(l => l.Coordinates).ToListAsync();
        }

        public static async Task<IActionResult> UpdateLocation(
            AppDbContext dbContext,
            int locationId,
            string? name,
            double? longitude,
            double? latitude)
        {
            var location = await dbContext.Locations.FindAsync(locationId);
            if (location is null) return new NotFoundResult();

            location.Name = name ?? location.Name;

            if (longitude.HasValue && latitude.HasValue)
            {
                location.Coordinates.Longitude = (double)longitude;
                location.Coordinates.Latitude = (double)latitude;
            }

            await dbContext.SaveChangesAsync();
            return new OkObjectResult(location);
        }

    }
}
