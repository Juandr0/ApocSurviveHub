using ApocSurviveHub.API.Services;

namespace ApocSurviveHub.API.Mappers;

public abstract class ItemMapper
{
    private const string ItemTag = "Item";

    public static void MapItemActions(WebApplication app)
    {
        app.MapPost("/Item", (ItemService itemService, string name, string type, int? locationId) =>
{
    return itemService.CreateItem(name, type, locationId);
}).WithTags(ItemTag)
.WithOpenApi(operation => new(operation)
{
    Summary = "Create an Item",
    Description = "Create a new item with the specified details.",
})
.WithOpenApi(generatedOperation =>
{
    generatedOperation.Parameters[0].Description = "The name of the item.";
    generatedOperation.Parameters[1].Description = "The type of the item.";
    generatedOperation.Parameters[2].Description = "The ID of the location where the item is located.";
    return generatedOperation;
});

        app.MapGet("/Item/Get/All", (ItemService itemService) =>
        {
            return itemService.GetItems();
        }).WithTags(ItemTag)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get All Items",
            Description = "Retrieve a list of all items.",
        });

        app.MapGet("/Item/Get/ById", (ItemService itemService, int itemId) =>
        {
            return itemService.GetById(itemId);
        }).WithTags(ItemTag)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get item by ID",
            Description = "Retrieve an item by entering itemId.",
        })
        .WithOpenApi(generatedOperation =>
        {
            generatedOperation.Parameters[0].Description = "The ID of the item to retrieve.";
            return generatedOperation;
        });

        app.MapPut("/Item", (ItemService itemService, int itemId, string? name, string? type, int? locationId) =>
        {
            return itemService.UpdateItem(itemId, name, type, locationId);
        }).WithTags(ItemTag)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Update Item",
            Description = "Update an existing item with the specified details.",
        })
        .WithOpenApi(generatedOperation =>
        {
            generatedOperation.Parameters[0].Description = "The ID of the item to update.";
            generatedOperation.Parameters[1].Description = "The updated name of the item.";
            generatedOperation.Parameters[2].Description = "The updated type of the item.";
            return generatedOperation;
        });

        app.MapDelete("/Item", (ItemService itemService, int itemId) =>
        {
            return itemService.DeleteItem(itemId);
        }).WithTags(ItemTag)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Delete Item",
            Description = "Delete an item by entering itemId.",
        })
        .WithOpenApi(generatedOperation =>
        {
            generatedOperation.Parameters[0].Description = "The ID of the item to delete.";
            return generatedOperation;
        });
    }
}