using ApocSurviveHub.API.Services;

namespace ApocSurviveHub.API.Mappers;

public abstract class SurvivorMapper
{
    private const string SurvivorTag = "Survivor";

    public static void MapSurvivorActions(WebApplication app)
    {
        app.MapPost("/Survivor", (SurvivorService survivorService, string Name, bool IsAlive, int? locationId) =>
        {
            return survivorService.CreateSurvivor(Name, IsAlive, locationId);
        })
        .WithTags(SurvivorTag)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Create a Survivor",
            Description = "Create a new survivor with the specified details.",
        })
        .WithOpenApi(generatedOperation =>
        {
            generatedOperation.Parameters[0].Description = "The name of the survivor.";
            generatedOperation.Parameters[1].Description = "The survival status of the survivor.";
            generatedOperation.Parameters[2].Description = "The ID of the location where the survivor is located.";
            return generatedOperation;
        });

        app.MapGet("/Survivor/Get/All", (SurvivorService survivorService) =>
        {
            return survivorService.GetSurvivors();
        })
        .WithTags(SurvivorTag)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get All Survivors",
            Description = "Retrieve a list of all survivors.",
        });

        app.MapGet("/Survivor/Get/ById", (SurvivorService survivorService, int survivorId) =>
        {
            return survivorService.GetById(survivorId);
        })
        .WithTags(SurvivorTag)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get survivor by ID",
            Description = "Retrieve a survivor by entering survivorId.",
        })
        .WithOpenApi(generatedOperation =>
        {
            generatedOperation.Parameters[0].Description = "The ID of the survivor to retrieve.";
            return generatedOperation;
        });

        app.MapPut("/Survivor", (SurvivorService survivorService, int survivorId, string? Name, bool? IsAlive, int? locationId) =>
        {
            return survivorService.UpdateSurvivor(survivorId, Name, IsAlive, locationId);
        })
        .WithTags(SurvivorTag)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Update Survivor",
            Description = "Update an existing survivor with the specified details.",
        })
        .WithOpenApi(generatedOperation =>
        {
            generatedOperation.Parameters[0].Description = "The ID of the survivor to update.";
            generatedOperation.Parameters[1].Description = "The updated name of the survivor.";
            generatedOperation.Parameters[2].Description = "The updated survival status of the survivor.";
            generatedOperation.Parameters[3].Description = "The updated ID of the location where the survivor is located.";
            return generatedOperation;
        });

        app.MapDelete("/Survivor", (SurvivorService survivorService, int survivorId) =>
        {
            return survivorService.DeleteSurvivor(survivorId);
        })
        .WithTags(SurvivorTag)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Delete Survivor",
            Description = "Delete a survivor by entering survivorId.",
        })
        .WithOpenApi(generatedOperation =>
        {
            generatedOperation.Parameters[0].Description = "The ID of the survivor to delete.";
            return generatedOperation;
        });

        app.MapPut("/Survivor/Inventory/Add", (SurvivorService survivorService, ItemService itemService, int survivorId, int itemId) =>
        {
            return survivorService.AddItem(survivorId, itemId);
        }).WithTags(SurvivorTag)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Add Item to Survivor Inventory",
            Description = "Add an item to the inventory of the specified survivor.",
        })
        .WithOpenApi(generatedOperation =>
        {
            generatedOperation.Parameters[0].Description = "The ID of the survivor to add the item to.";
            generatedOperation.Parameters[1].Description = "The ID of the item to add to the survivor's inventory.";
            return generatedOperation;
        });

        app.MapPut("/Survivor/Inventory/Remove", (SurvivorService survivorService, ItemService itemService, int survivorId, int itemId) =>
        {
            return survivorService.RemoveItem(survivorId, itemId);
        }).WithTags(SurvivorTag)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Remove Item from Survivor Inventory",
            Description = "Remove an item from the inventory of the specified survivor.",
        })
        .WithOpenApi(generatedOperation =>
        {
            generatedOperation.Parameters[0].Description = "The ID of the survivor to remove the item from.";
            generatedOperation.Parameters[1].Description = "The ID of the item to remove from the survivor's inventory.";
            return generatedOperation;
        });

    }
}