using System.Text.Json.Serialization;
using ApocSurviveHub.API.Models;
using ApocSurviveHub.API.Data;
using ApocSurviveHub.API.Interfaces;
using ApocSurviveHub.API.Repository;
using ApocSurviveHub.API.Services;
using ApocSurviveHub.API.Mappers;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Apoc Survive Hub API",
        Description = "The Apocalypse Survive Hub API empowers users to navigate a post-apocalyptic world, manage survivors, locations, items, and hordes. Create resilient survivors, assign them locations, track essential items, and strategize against menacing hordes. This API provides a comprehensive toolkit for survival management in the face of catastrophe."
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "ApocSurviveHub.API.xml"));
});
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

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Maps the endpoint actions 
SurvivorMapper.MapSurvivorActions(app);
HordeMapper.MapHordeActions(app);
ItemMapper.MapItemActions(app);
LocationMapper.MapLocationActions(app);

app.Run();