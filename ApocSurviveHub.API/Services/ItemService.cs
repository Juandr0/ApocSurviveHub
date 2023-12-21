using ApocSurviveHub.API.Models;
using ApocSurviveHub.API.Data;
using Microsoft.AspNetCore.Mvc;

namespace Services;

public abstract class ItemService
{
    public static async Task<IActionResult> CreateItem(
            AppDbContext dbContext,
            int? survivorId,
            string name,
            string type)
    {
        // var survivor = await dbContext.Survivors.FindAsync(survivorId);
        // if (survivor is null) return new NotFoundResult();

        var item = new Item(name, type);
        dbContext.Items.Add(item);

        await dbContext.SaveChangesAsync();

        return new CreatedResult($"/Item/{item.Id}", item);
    }

    public static IEnumerable<Item> GetItems(AppDbContext dbContext)
    {
        return dbContext.Items.ToList();
    }

    public static async Task<IActionResult> UpdateItem(AppDbContext dbContext, int itemId, string? name, string? type)
    {
        var item = await dbContext.Items.FindAsync(itemId);
        if (item is null) return new NotFoundResult();

        item.Name = name ?? item.Name;
        item.Type = type ?? item.Type;

        await dbContext.SaveChangesAsync();
        return new OkObjectResult(item);
    }

    public static async Task<IActionResult> DeleteItem(AppDbContext dbContext, int itemId)
    {
        var item = await dbContext.Items.FindAsync(itemId);
        if (item is null) return new NotFoundResult();

        dbContext.Items.Remove(item);
        await dbContext.SaveChangesAsync();

        return new OkObjectResult(item);
    }
}
