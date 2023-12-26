using System.Text.Json.Serialization;
using ApocSurviveHub.API.Models;
using ApocSurviveHub.API.Data;
using Microsoft.EntityFrameworkCore;
using ApocSurviveHub.API.Interfaces;
using ApocSurviveHub.API.Repository;
using ApocSurviveHub.API.Services;
using Microsoft.AspNetCore.Mvc;

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

builder.Services.AddScoped<ICrud<Survivor>, CrudRepository<Survivor>>();
builder.Services.AddScoped<SurvivorService>();

builder.Services.AddScoped<ICrud<Item>, CrudRepository<Item>>();
builder.Services.AddScoped<ItemService>();

builder.Services.AddScoped<ICrud<Location>, CrudRepository<Location>>();
builder.Services.AddScoped<LocationService>();

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

app.MapPost("/Survivor", (SurvivorService survivorService, string Name, bool IsAlive, int? locationId) =>
{
    return survivorService.CreateSurvivor(Name, IsAlive, locationId);
});

app.MapGet("/Survivor/Get/All", (SurvivorService survivorService) =>
{
    return survivorService.GetSurvivors();
});

app.MapGet("/Survivor/Get/ById", (SurvivorService survivorService, int survivorId) =>
{
    return survivorService.GetById(survivorId);
});

app.MapPut("/Survivor", (SurvivorService survivorService, int survivorId, string? Name, bool? IsAlive, int? locationId) =>
{
    return survivorService.UpdateSurvivor(survivorId, Name, IsAlive, locationId);
});

app.MapDelete("/Survivor", (SurvivorService survivorService, int survivorId) =>
{
    return survivorService.DeleteSurvivor(survivorId);
});

app.MapPut("/Survivor/Inventory/Add", (SurvivorService survivorService, ItemService itemService, int survivorId, int itemId) =>
{
    return survivorService.AddItem(survivorId, itemId);
});

app.MapPut("/Survivor/Inventory/Remove", (SurvivorService survivorService, ItemService itemService, int survivorId, int itemId) =>
{
    return survivorService.RemoveItem(survivorId, itemId);
});

/////////////////////
/// HORDES START ////
/////////////////////

app.MapPost("/Horde/Add", (HordeService hordeService, string name, int threatLevel, int? locationId) =>
{
    return hordeService.CreateHorde(name, threatLevel, locationId);
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

app.MapPost("/Item", (ItemService itemService, string name, string type, int? locationId) =>
{
    return itemService.CreateItem(name, type, locationId);
});

app.MapGet("/Item/Get/All", (ItemService itemService) =>
{
    return itemService.GetItems();
});

app.MapGet("/Item/Get/ById", (ItemService itemService, int itemId) =>
{
    return itemService.GetById(itemId);
});

app.MapPut("/Item", (ItemService itemService, int itemId, string? name, string? type) =>
{
    return itemService.UpdateItem(itemId, name, type);
});

app.MapDelete("/Item", (ItemService itemService, int itemId) =>
{
    return itemService.DeleteItem(itemId);
});


/////////////////////
// LOCATIONS START //
/////////////////////

app.MapPost("/Location", (LocationService locationService, string name, double longitude, double latitude) =>
{
    return locationService.CreateLocation(name, longitude, latitude);
});

app.MapGet("/Location/Get/all", (LocationService locationService) =>
{
    return locationService.GetLocations();
});

app.MapGet("/Location/Get/ById", (LocationService locationService, int locationId) =>
{
    return locationService.GetById(locationId);
});

app.MapPut("/Location", (LocationService locationService, int locationId, string? name, double? longitude, double? latitude) =>
{
    return locationService.UpdateLocation(locationId, name, longitude, latitude);
});


app.Run();
