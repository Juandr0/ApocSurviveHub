using ApocSurviveHub.API.Models;
using ApocSurviveHub.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApocSurviveHub.API.Interfaces;

namespace ApocSurviveHub.API.Services;

public class ItemService
{

    private readonly ICrud<Item> _itemRepository;

    public ItemService(ICrud<Item> itemRepository)
    {
        _itemRepository = itemRepository;
    }
    public static async Task<IActionResult> CreateItem(
            AppDbContext dbContext,
            string name,
            string type,
            int? locationId)
    {
        var item = new Item(name, type, locationId);
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
        var item = await dbContext.Items.
        Include(i => i.LocationId).
        Include(i => i.Location).
        FirstOrDefaultAsync(i => i.Id == itemId);

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
