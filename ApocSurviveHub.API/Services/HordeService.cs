using ApocSurviveHub.API.Models;
using ApocSurviveHub.API.Data;
using Microsoft.AspNetCore.Mvc;

namespace Services;

public abstract class HordeService
{
    public static async Task<IActionResult> CreateHorde(
        AppDbContext dbContext,
        string Name,
        int ThreatLevel,
        string lastSeen,
        double _latitude,
        double _longitude)
    {
        var location = new Location(lastSeen, _longitude, _latitude);

        dbContext.Locations.Add(location);
        await dbContext.SaveChangesAsync();

        var horde = new Horde(Name, ThreatLevel, location.Id);
        dbContext.Hordes.Add(horde);
        await dbContext.SaveChangesAsync();

        return new CreatedResult($"/Horde/{horde.Id}", horde);
    }

    public static IEnumerable<Horde> GetHordes(AppDbContext dbContext)
    {
        return dbContext.Hordes.ToList();
    }

    public static async Task<IActionResult> UpdateHorde(
        AppDbContext dbContext,
        int hordeId,
        string? Name,
        int? ThreatLevel,
        string? lastSeen,
        double? latitude,
        double? longitude)
    {
        var horde = await dbContext.Hordes.FindAsync(hordeId);
        if (horde is null) return new NotFoundResult();

        horde.Name = Name ?? horde.Name;
        horde.ThreatLevel = ThreatLevel ?? horde.ThreatLevel;

        if (lastSeen != null && latitude.HasValue && longitude.HasValue)
        {
            var newLocation = new Location(
                name: lastSeen,
                longitude: (double)longitude,
                latitude: (double)latitude
            );

            dbContext.Locations.Add(newLocation);
            await dbContext.SaveChangesAsync();
            horde.LocationId = newLocation.Id;
        }

        await dbContext.SaveChangesAsync();
        return new OkObjectResult(horde);
    }

    public static async Task<IActionResult> DeleteHorde(AppDbContext dbContext, int hordeId)
    {
        var horde = await dbContext.Hordes.FindAsync(hordeId);
        if (horde is null) return new NotFoundResult();

        dbContext.Hordes.Remove(horde);
        await dbContext.SaveChangesAsync();

        return new OkObjectResult(horde);
    }
}
