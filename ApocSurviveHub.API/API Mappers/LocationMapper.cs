using ApocSurviveHub.API.Services;

namespace ApocSurviveHub.API.Mappers;

public abstract class LocationMapper
{
    private const string LocationTag = "Location";

    public static void MapLocationActions(WebApplication app)
    {

        app.MapPost("/Location", (LocationService locationService, string name, double longitude, double latitude) =>
        {
            return locationService.CreateLocation(name, longitude, latitude);
        }).WithTags(LocationTag)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Create a Location",
            Description = "Create a new location with the specified details.",
        })
        .WithOpenApi(generatedOperation =>
        {
            generatedOperation.Parameters[0].Description = "The name of the location.";
            generatedOperation.Parameters[1].Description = "The longitude of the location.";
            generatedOperation.Parameters[2].Description = "The latitude of the location.";
            return generatedOperation;
        });

        app.MapGet("/Location/Get/All", (LocationService locationService) =>
        {
            return locationService.GetLocations();
        }).WithTags(LocationTag)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get All Locations",
            Description = "Retrieve a list of all locations.",
        });

        app.MapGet("/Location/Get/ById", (LocationService locationService, int locationId) =>
        {
            return locationService.GetById(locationId);
        }).WithTags(LocationTag)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get location by ID",
            Description = "Retrieve a location by entering locationId.",
        })
        .WithOpenApi(generatedOperation =>
        {
            generatedOperation.Parameters[0].Description = "The ID of the location to retrieve.";
            return generatedOperation;
        });

        app.MapPut("/Location", (LocationService locationService, int locationId, string? name, double? longitude, double? latitude) =>
        {
            return locationService.UpdateLocation(locationId, name, longitude, latitude);
        }).WithTags(LocationTag)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Update Location",
            Description = "Update an existing location with the specified details.",
        })
        .WithOpenApi(generatedOperation =>
        {
            generatedOperation.Parameters[0].Description = "The ID of the location to update.";
            generatedOperation.Parameters[1].Description = "The updated name of the location.";
            generatedOperation.Parameters[2].Description = "The updated longitude of the location.";
            generatedOperation.Parameters[3].Description = "The updated latitude of the location.";
            return generatedOperation;
        });

        app.MapDelete("/Location", (LocationService locationService, int locationId) =>
        {
            return locationService.DeleteLocation(locationId);
        }).WithTags(LocationTag)
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Delete Location",
            Description = "Delete a location by entering locationId.",
        })
        .WithOpenApi(generatedOperation =>
        {
            generatedOperation.Parameters[0].Description = "The ID of the location to delete.";
            return generatedOperation;
        });
    }
}