using System.Text.Json.Serialization;
using ApocSurviveHub.API.Models;
using ApocSurviveHub.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// Survivors 

app.MapPost("/Survivor", async (
    AppDbContext dbContext,
    string Name,
    bool IsAlive,
    double _latitude,
    double _longitude,
    string locationName) =>
{
    var coordinates = new Coordinates { Latitude = _latitude, Longitude = _longitude };
    var location = new Location(locationName, _longitude, _latitude);

    dbContext.Locations.Add(location);
    await dbContext.SaveChangesAsync();

    var survivor = new Survivor(Name, IsAlive, location.Id);
    dbContext.Survivors.Add(survivor);
    await dbContext.SaveChangesAsync();

    return Results.Created($"/Survivor/{survivor.Id}", survivor);
});

app.MapGet("/Survivor", (AppDbContext dbContext) =>
{
    var survivors = dbContext.Survivors.ToList();
    return survivors;
});

app.MapPut("/Survivor", async (
    AppDbContext dbContext,
    int survivorId,
    string? Name,
    bool? IsAlive,
    string? locationName,
    double? _latitude,
    double? _longitude
    ) =>
{
    var survivor = await dbContext.Survivors.FindAsync(survivorId);
    if (survivor is null) return Results.NotFound();

    survivor.Name = Name ?? survivor.Name;
    survivor.IsAlive = IsAlive ?? survivor.IsAlive;

    if (locationName != null && _latitude.HasValue && _longitude.HasValue)
    {
        survivor.CurrentLocation = new Location(
            name: locationName,
            longitude: (double)_longitude,
            latitude: (double)_latitude
            );
    }


    await dbContext.SaveChangesAsync();

    return Results.NoContent();
});

// app.MapDelete("/Survivor")) {

// }



app.Run();
