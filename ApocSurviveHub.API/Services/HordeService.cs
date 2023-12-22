using ApocSurviveHub.API.Models;
using ApocSurviveHub.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Services;

public abstract class HordeService
{
    public static async Task<IActionResult> CreateHorde(
        AppDbContext dbContext,
        string Name,
        int ThreatLevel,
        int? locationId)
    {
        var horde = new Horde(Name, ThreatLevel, locationId);

        if (locationId.HasValue)
        {
            horde.LocationId = locationId.Value;
        }

        dbContext.Hordes.Add(horde);
        await dbContext.SaveChangesAsync();

        return new CreatedResult($"/Horde/{horde.Id}", horde);
    }

    public static IEnumerable<Horde> GetHordes(AppDbContext dbContext)
    {
        return dbContext.Hordes.Include(h => h.Location).ThenInclude(l => l!.Coordinates).ToList();
    }

    public static async Task<IActionResult> UpdateHorde(
        AppDbContext dbContext,
        int hordeId,
        string? Name,
        int? ThreatLevel,
        int? locationId)
    {
        var horde = await dbContext.Hordes.FindAsync(hordeId);
        if (horde is null) return new NotFoundResult();

        horde.Name = Name ?? horde.Name;
        horde.ThreatLevel = ThreatLevel ?? horde.ThreatLevel;

        if (locationId.HasValue)
        {
            horde.LocationId = locationId.Value;
            var getLocationFromId = await dbContext.Locations
            .Include(l => l.Coordinates)
            .FirstOrDefaultAsync(l => l.Id == locationId);
            horde.Location = getLocationFromId;
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
