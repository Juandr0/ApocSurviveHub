using System.Text.Json.Serialization;
using ApocSurviveHub.API.Models;
using ApocSurviveHub.API.Data;
using Microsoft.EntityFrameworkCore;
using Services;

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

app.MapPost("/Survivor", (AppDbContext dbContext, string Name, bool IsAlive, string locationName, double _latitude, double _longitude) =>
{
    return SurvivorService.CreateSurvivor(dbContext, Name, IsAlive, locationName, _latitude, _longitude);
});

app.MapGet("/Survivor", (AppDbContext dbContext) =>
{
    return SurvivorService.GetSurvivors(dbContext);
});

app.MapPut("/Survivor", (AppDbContext dbContext, int survivorId, string? Name, bool? IsAlive, string? locationName, double? latitude, double? longitude) =>
{
    return SurvivorService.UpdateSurvivor(dbContext, survivorId, Name, IsAlive, locationName, latitude, longitude);
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


// Hordes

app.MapPost("/Horde", (AppDbContext dbContext, string Name, int ThreatLevel, string lastSeen, double _latitude, double _longitude) =>
{
    return HordeService.CreateHorde(dbContext, Name, ThreatLevel, lastSeen, _latitude, _longitude);
});

app.MapGet("/Horde", (AppDbContext dbContext) =>
{
    return HordeService.GetHordes(dbContext);
});

app.MapPut("/Horde", (AppDbContext dbContext, int hordeId, string? Name, int? ThreatLevel, string? lastSeen, double? latitude, double? longitude) =>
{
    return HordeService.UpdateHorde(dbContext, hordeId, Name, ThreatLevel, lastSeen, latitude, longitude);
});

app.MapDelete("/Horde", (AppDbContext dbContext, int hordeId) =>
{
    return HordeService.DeleteHorde(dbContext, hordeId);
});

// Items 

app.MapPost("/Item", (AppDbContext dbContext, string name, string type) =>
{
    return ItemService.CreateItem(dbContext, null, name, type);
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



app.Run();
