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
}
