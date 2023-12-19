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




app.Run();
