using System.Text.Json.Serialization;
using ApocSurviveHub.API.Models;
using ApocSurviveHub.API.Data;
using Microsoft.EntityFrameworkCore;
using ApocSurviveHub.API.Interfaces;
using ApocSurviveHub.API.Repository;
using ApocSurviveHub.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddDbContext<AppDbContext>();

// Repositories
builder.Services.AddScoped<ICrud<Horde>, CrudRepository<Horde>>();
builder.Services.AddScoped<HordeService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/////////////////////
// SURVIVORS START //
/////////////////////

app.MapPost("/Survivor", (AppDbContext dbContext, string Name, bool IsAlive, int? locationId) =>
{
    return SurvivorService.CreateSurvivor(dbContext, Name, IsAlive, locationId);
});

app.MapGet("/Survivor", (AppDbContext dbContext) =>
{
    return SurvivorService.GetSurvivors(dbContext);
});

app.MapPut("/Survivor", (AppDbContext dbContext, int survivorId, string? Name, bool? IsAlive, int? locationId) =>
{
    return SurvivorService.UpdateSurvivor(dbContext, survivorId, Name, IsAlive, locationId);
});

app.MapDelete("/Survivor", (AppDbContext dbContext, int survivorId) =>
{
    return SurvivorService.DeleteSurvivor(dbContext, survivorId);
});

app.MapPut("/Survivor/Inventory/Add", (AppDbContext dbContext, int survivorId, int itemId) =>
{
    return SurvivorService.AddItem(dbContext, survivorId, itemId);
});

app.MapPut("/Survivor/Inventory/Remove", (AppDbContext dbContext, int survivorId, int itemId) =>
{
    return SurvivorService.RemoveItem(dbContext, survivorId, itemId);
});

/////////////////////
/// HORDES START ////
/////////////////////

app.MapPost("/Horde/Add", (HordeService hordeService, string name, int threatLevel, int? locationId) =>
{
    hordeService.CreateHorde(name, threatLevel, locationId);
});

app.MapGet("/Horde/Get/All", (HordeService hordeService) =>
{
    return hordeService.GetHordes();
});

app.MapGet("/Horde/Get/ById", (HordeService hordeService, int id) =>
{
    return hordeService.GetById(id);
});

app.MapPut("/Horde", (HordeService hordeService, int hordeId, string? name, int? threatLevel, int? locationId) =>
{
    hordeService.UpdateHorde(hordeId, name, threatLevel, locationId);
});

app.MapDelete("/Horde/", (HordeService hordeService, int hordeId) =>
{
    hordeService.DeleteHorde(hordeId);
});

/////////////////////
//// ITEMS START ////
/////////////////////

app.MapPost("/Item", (AppDbContext dbContext, string name, string type, int? locationId) =>
{
    return ItemService.CreateItem(dbContext, name, type, locationId);
});

app.MapGet("/Item", (AppDbContext dbContext) =>
{
    return ItemService.GetItems(dbContext);
});

app.MapPut("/Item", (AppDbContext dbContext, int itemId, string? name, string? type) =>
{
    return ItemService.UpdateItem(dbContext, itemId, name, type);
});

app.MapDelete("/Item", (AppDbContext dbContext, int itemId) =>
{
    return ItemService.DeleteItem(dbContext, itemId);
});


/////////////////////
// LOCATIONS START //
/////////////////////

app.MapPost("/Location", (AppDbContext dbContext, string name, double longitude, double latitude) =>
{
    return LocationService.CreateLocation(dbContext, name, longitude, latitude);
});

app.MapGet("/Location", (AppDbContext dbContext) =>
{
    return LocationService.GetLocations(dbContext);
});

app.MapPut("/Location", (AppDbContext dbContext, int locationId, string? name, double? longitude, double? latitude) =>
{
    return LocationService.UpdateLocation(dbContext, locationId, name, longitude, latitude);
});


app.Run();
