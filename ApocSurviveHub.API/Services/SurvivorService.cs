using ApocSurviveHub.API.Models;
using ApocSurviveHub.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApocSurviveHub.API.Services;

public abstract class SurvivorService
{
    public static async Task<IActionResult> CreateSurvivor(
    AppDbContext dbContext,
    string Name,
    bool IsAlive,
    int? locationId)
    {
        var survivor = new Survivor(Name, IsAlive, locationId);

        if (locationId.HasValue)
        {
            survivor.LocationId = locationId.Value;
        }

        dbContext.Survivors.Add(survivor);
        await dbContext.SaveChangesAsync();

        return new CreatedResult($"/Survivor/{survivor.Id}", survivor);
    }
    public static IEnumerable<Survivor> GetSurvivors(AppDbContext dbContext)
    {
        return dbContext.Survivors.Include(s => s.Inventory)
        .Include(s => s.Location)
        .ThenInclude(l => l!.Coordinates).ToList();
    }

    public static async Task<IActionResult> UpdateSurvivor(
        AppDbContext dbContext,
        int survivorId,
        string? Name,
        bool? IsAlive,
        int? locationId)
    {
        var survivor = await dbContext.Survivors
            .Include(s => s.Inventory)
            .Include(s => s.Location)
            .FirstOrDefaultAsync(s => s.Id == survivorId);
        if (survivor is null) return new NotFoundResult();

        survivor.Name = Name ?? survivor.Name;
        survivor.IsAlive = IsAlive ?? survivor.IsAlive;

        if (locationId.HasValue)
        {
            survivor.LocationId = locationId;
            foreach (var item in survivor.Inventory)
            {
                item.LocationId = locationId;
            }
        }

        await dbContext.SaveChangesAsync();
        return new OkObjectResult(survivor);
    }

    public static async Task<IActionResult> DeleteSurvivor(AppDbContext dbContext, int survivorId)
    {
        var survivor = await dbContext.Survivors.FindAsync(survivorId);
        if (survivor is null) return new NotFoundResult();

        dbContext.Survivors.Remove(survivor);
        await dbContext.SaveChangesAsync();
        return new OkObjectResult(survivor);
    }

    // Survivor item handling

    public static async Task<IActionResult> AddItem(AppDbContext dbContext, int survivorId, int itemId)
    {
        var survivor = await dbContext.Survivors.FindAsync(survivorId);
        if (survivor is null) return new NotFoundResult();

        var item = await dbContext.Items.FindAsync(itemId);
        if (item is null) return new NotFoundResult();

        if (item.LocationId is not null)
        {
            if (item.LocationId == survivor.LocationId)
            {
                item.LocationId = survivor.LocationId;
                item.Location = survivor.Location;
                await dbContext.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("Survivor and Item is not at the same place");
            }
        }
        else
        {
            item.LocationId = survivor.LocationId;
            item.Location = survivor.Location;
            survivor.Inventory.Add(item);
            await dbContext.SaveChangesAsync();
        }

        return new OkObjectResult(survivor);
    }

    public static async Task<IActionResult> RemoveItem(AppDbContext dbContext, int survivorId, int itemId)
    {
        var survivor = await dbContext.Survivors.FindAsync(survivorId);
        if (survivor is null) return new NotFoundResult();

        var item = await dbContext.Items.FindAsync(itemId);
        if (item is null) return new NotFoundResult();

        survivor.Inventory.Remove(item);
        await dbContext.SaveChangesAsync();

        return new OkObjectResult(survivor);
    }
}

