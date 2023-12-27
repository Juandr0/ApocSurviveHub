using System.Text.Json.Serialization;
using ApocSurviveHub.API.Models;
using ApocSurviveHub.API.Data;
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
/////// Tags ////////
var survivorTag = "Survivor";
var hordeTag = "Horde";
var itemTag = "Item";
var locationTag = "Location";


/////////////////////
// SURVIVORS START //
/////////////////////

app.MapPost("/Survivor", (SurvivorService survivorService, string Name, bool IsAlive, int? locationId) =>
{
    return survivorService.CreateSurvivor(Name, IsAlive, locationId);
}).WithTags(survivorTag);

app.MapGet("/Survivor/Get/All", (SurvivorService survivorService) =>
{
    return survivorService.GetSurvivors();
}).WithTags(survivorTag);

app.MapGet("/Survivor/Get/ById", (SurvivorService survivorService, int survivorId) =>
{
    return survivorService.GetById(survivorId);
}).WithTags(survivorTag);

app.MapPut("/Survivor", (SurvivorService survivorService, int survivorId, string? Name, bool? IsAlive, int? locationId) =>
{
    return survivorService.UpdateSurvivor(survivorId, Name, IsAlive, locationId);
}).WithTags(survivorTag);

app.MapDelete("/Survivor", (SurvivorService survivorService, int survivorId) =>
{
    return survivorService.DeleteSurvivor(survivorId);
}).WithTags(survivorTag);

app.MapPut("/Survivor/Inventory/Add", (SurvivorService survivorService, ItemService itemService, int survivorId, int itemId) =>
{
    return survivorService.AddItem(survivorId, itemId);
}).WithTags(survivorTag);

app.MapPut("/Survivor/Inventory/Remove", (SurvivorService survivorService, ItemService itemService, int survivorId, int itemId) =>
{
    return survivorService.RemoveItem(survivorId, itemId);
}).WithTags(survivorTag);

/////////////////////
/// HORDES START ////
/////////////////////

app.MapPost("/Horde", (HordeService hordeService, string name, int threatLevel, int? locationId) =>
{
    return hordeService.CreateHorde(name, threatLevel, locationId);
}).WithTags(hordeTag);

app.MapGet("/Horde/Get/All", (HordeService hordeService) =>
{
    return hordeService.GetHordes();
}).WithTags(hordeTag);

app.MapGet("/Horde/Get/ById", (HordeService hordeService, int id) =>
{
    return hordeService.GetById(id);
}).WithTags(hordeTag);

app.MapPut("/Horde", (HordeService hordeService, int hordeId, string? name, int? threatLevel, int? locationId) =>
{
    hordeService.UpdateHorde(hordeId, name, threatLevel, locationId);
}).WithTags(hordeTag);

app.MapDelete("/Horde", (HordeService hordeService, int hordeId) =>
{
    hordeService.DeleteHorde(hordeId);
}).WithTags(hordeTag);

/////////////////////
//// ITEMS START ////
/////////////////////

app.MapPost("/Item", (ItemService itemService, string name, string type, int? locationId) =>
{
    return itemService.CreateItem(name, type, locationId);
}).WithTags(itemTag);

app.MapGet("/Item/Get/All", (ItemService itemService) =>
{
    return itemService.GetItems();
}).WithTags(itemTag);

app.MapGet("/Item/Get/ById", (ItemService itemService, int itemId) =>
{
    return itemService.GetById(itemId);
}).WithTags(itemTag);

app.MapPut("/Item", (ItemService itemService, int itemId, string? name, string? type) =>
{
    return itemService.UpdateItem(itemId, name, type);
}).WithTags(itemTag);

app.MapDelete("/Item", (ItemService itemService, int itemId) =>
{
    return itemService.DeleteItem(itemId);
}).WithTags(itemTag);


/////////////////////
// LOCATIONS START //
/////////////////////

app.MapPost("/Location", (LocationService locationService, string name, double longitude, double latitude) =>
{
    return locationService.CreateLocation(name, longitude, latitude);
}).WithTags(locationTag);

app.MapGet("/Location/Get/All", (LocationService locationService) =>
{
    return locationService.GetLocations();
}).WithTags(locationTag);

app.MapGet("/Location/Get/ById", (LocationService locationService, int locationId) =>
{
    return locationService.GetById(locationId);
}).WithTags(locationTag);

app.MapPut("/Location", (LocationService locationService, int locationId, string? name, double? longitude, double? latitude) =>
{
    return locationService.UpdateLocation(locationId, name, longitude, latitude);
}).WithTags(locationTag);

app.MapDelete("/Location", (LocationService locationService, int locationId) =>
{
    return locationService.DeleteLocation(locationId);
}).WithTags(locationTag);


app.Run();
