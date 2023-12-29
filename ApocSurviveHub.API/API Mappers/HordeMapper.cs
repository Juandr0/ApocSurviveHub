using ApocSurviveHub.API.Services;

namespace ApocSurviveHub.API.Mappers;

public abstract class HordeMapper
{
    private static string hordeTag = "Horde";
    public static void MapHordeActions(WebApplication app)
    {

        app.MapPost("/Horde", (HordeService hordeService, string name, int threatLevel, int? locationId) =>
        {
            return hordeService.CreateHorde(name, threatLevel, locationId);
        }).WithTags(hordeTag)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Create a Horde",
            Description = "Create a new horde with the specified details.",
        })
        .WithOpenApi(generatedOperation =>
        {
            generatedOperation.Parameters[0].Description = "The name of the horde.";
            generatedOperation.Parameters[1].Description = "The threat level of the horde.";
            generatedOperation.Parameters[2].Description = "The ID of the location where the horde is located.";
            return generatedOperation;
        });

        app.MapGet("/Horde/Get/All", (HordeService hordeService) =>
        {
            return hordeService.GetHordes();
        }).WithTags(hordeTag)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get All Hordes",
            Description = "Retrieve a list of all hordes.",
        });

        app.MapGet("/Horde/Get/ById", (HordeService hordeService, int id) =>
        {
            return hordeService.GetById(id);
        }).WithTags(hordeTag)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get horde by ID",
            Description = "Retrieve a horde by entering hordeId",
        })
        .WithOpenApi(generatedOperation =>
        {
            generatedOperation.Parameters[0].Description = "The ID of the horde to retrieve.";
            return generatedOperation;
        });

        app.MapPut("/Horde", (HordeService hordeService, int hordeId, string? name, int? threatLevel, int? locationId) =>
        {
            hordeService.UpdateHorde(hordeId, name, threatLevel, locationId);
        }).WithTags(hordeTag)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Update Horde",
            Description = "Update an existing horde with the specific details.",
        })
        .WithOpenApi(generatedOperation =>
        {
            generatedOperation.Parameters[0].Description = "The ID of the horde to update.";
            generatedOperation.Parameters[1].Description = "The updated name of the horde.";
            generatedOperation.Parameters[2].Description = "The updated threat level of the horde.";
            generatedOperation.Parameters[3].Description = "The updated ID of the location where the horde is located.";
            return generatedOperation;
        });

        app.MapDelete("/Horde", (HordeService hordeService, int hordeId) =>
        {
            hordeService.DeleteHorde(hordeId);
        }).WithTags(hordeTag)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Delete Horde",
            Description = "Delete a horde by entering hordeId.",
        })
        .WithOpenApi(generatedOperation =>
        {
            generatedOperation.Parameters[0].Description = "The ID of the horde to delete.";
            return generatedOperation;
        });

    }

}