using ApocSurviveHub.API.Models;
using ApocSurviveHub.API.Data;
using Microsoft.AspNetCore.Mvc;

namespace Services;

public abstract class SurvivorService
{
    public static async Task<IActionResult> CreateSurvivor(
        AppDbContext dbContext,
        string Name,
        bool IsAlive,
        string locationName,
        double _latitude,
        double _longitude)
    {
        var location = new Location(locationName, _longitude, _latitude);

        dbContext.Locations.Add(location);
        await dbContext.SaveChangesAsync();

        var survivor = new Survivor(Name, IsAlive, location.Id);
        dbContext.Survivors.Add(survivor);
        await dbContext.SaveChangesAsync();

        return new CreatedResult($"/Survivor/{survivor.Id}", survivor);
    }

    public static IEnumerable<Survivor> GetSurvivors(AppDbContext dbContext)
    {
        return dbContext.Survivors.ToList();
    }

    public static async Task<IActionResult> UpdateSurvivor(
        AppDbContext dbContext,
        int survivorId,
        string? Name,
        bool? IsAlive,
        string? locationName,
        double? latitude,
        double? longitude)
    {
        var survivor = await dbContext.Survivors.FindAsync(survivorId);
        if (survivor is null) return new NotFoundResult();

        survivor.Name = Name ?? survivor.Name;
        survivor.IsAlive = IsAlive ?? survivor.IsAlive;

        if (locationName != null && latitude.HasValue && longitude.HasValue)
        {
            var newLocation = new Location(
                name: locationName,
                longitude: (double)longitude,
                latitude: (double)latitude
            );

            dbContext.Locations.Add(newLocation);
            await dbContext.SaveChangesAsync();
            survivor.LocationId = newLocation.Id;
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

    // Create Item for Survivor

    // public static async Task<IActionResult> CreateItem(
    //         AppDbContext dbContext,
    //         int survivorId,
    //         string name,
    //         string type)
    //     {
    //         var survivor = await dbContext.Survivors.FindAsync(survivorId);
    //         if (survivor is null) return new NotFoundResult();

    //         var item = new Item(name, type, survivorId);
    //         survivor.Inventory.Add(item);

    //         await dbContext.SaveChangesAsync();

    //         return new CreatedResult($"/Item/{item.Id}", item);
    //     }

        // Update Item for Survivor

        public static async Task<IActionResult> UpdateItem(
            AppDbContext dbContext,
            int itemId,
            string name,
            string type)
        {
            var item = await dbContext.Items.FindAsync(itemId);
            if (item is null) return new NotFoundResult();

            item.Name = name ?? item.Name;
            item.Type = type ?? item.Type;

            await dbContext.SaveChangesAsync();

            return new OkObjectResult(item);
        }

        // Delete Item for Survivor 

        public static async Task<IActionResult> DeleteItem(AppDbContext dbContext, int itemId)
        {
            var item = await dbContext.Items.FindAsync(itemId);
            if (item is null) return new NotFoundResult();

            dbContext.Items.Remove(item);
            await dbContext.SaveChangesAsync();

            return new OkObjectResult(item);
        }
    }

