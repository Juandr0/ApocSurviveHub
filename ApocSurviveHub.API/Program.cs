using System.Text.Json.Serialization;
using ApocSurviveHub.API.Models;
using ApocSurviveHub.API.Data;
using ApocSurviveHub.API.Interfaces;
using ApocSurviveHub.API.Repository;
using ApocSurviveHub.API.Services;
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

/////////////////////
/////// Tags ////////
/////////////////////

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
})
.WithTags(survivorTag)
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
.WithTags(survivorTag)
.WithOpenApi(operation => new(operation)
{
    Summary = "Get All Survivors",
    Description = "Retrieve a list of all survivors.",
});

app.MapGet("/Survivor/Get/ById", (SurvivorService survivorService, int survivorId) =>
{
    return survivorService.GetById(survivorId);
})
.WithTags(survivorTag)
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
.WithTags(survivorTag)
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
.WithTags(survivorTag)
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
}).WithTags(survivorTag)
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
}).WithTags(survivorTag)
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

/////////////////////
//// HORDES START ////
/////////////////////

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

/////////////////////
//// ITEMS START ////
/////////////////////

app.MapPost("/Item", (ItemService itemService, string name, string type, int? locationId) =>
{
    return itemService.CreateItem(name, type, locationId);
}).WithTags(itemTag)
.WithOpenApi(operation => new(operation)
{
    Summary = "Create an Item",
    Description = "Create a new item with the specified details.",
})
.WithOpenApi(generatedOperation =>
{
    generatedOperation.Parameters[0].Description = "The name of the item.";
    generatedOperation.Parameters[1].Description = "The type of the item.";
    generatedOperation.Parameters[2].Description = "The ID of the location where the item is located.";
    return generatedOperation;
});

app.MapGet("/Item/Get/All", (ItemService itemService) =>
{
    return itemService.GetItems();
}).WithTags(itemTag)
.WithOpenApi(operation => new(operation)
{
    Summary = "Get All Items",
    Description = "Retrieve a list of all items.",
});

app.MapGet("/Item/Get/ById", (ItemService itemService, int itemId) =>
{
    return itemService.GetById(itemId);
}).WithTags(itemTag)
.WithOpenApi(operation => new(operation)
{
    Summary = "Get item by ID",
    Description = "Retrieve an item by entering itemId.",
})
.WithOpenApi(generatedOperation =>
{
    generatedOperation.Parameters[0].Description = "The ID of the item to retrieve.";
    return generatedOperation;
});

app.MapPut("/Item", (ItemService itemService, int itemId, string? name, string? type) =>
{
    return itemService.UpdateItem(itemId, name, type);
}).WithTags(itemTag)
.WithOpenApi(operation => new(operation)
{
    Summary = "Update Item",
    Description = "Update an existing item with the specified details.",
})
.WithOpenApi(generatedOperation =>
{
    generatedOperation.Parameters[0].Description = "The ID of the item to update.";
    generatedOperation.Parameters[1].Description = "The updated name of the item.";
    generatedOperation.Parameters[2].Description = "The updated type of the item.";
    return generatedOperation;
});

app.MapDelete("/Item", (ItemService itemService, int itemId) =>
{
    return itemService.DeleteItem(itemId);
}).WithTags(itemTag)
.WithOpenApi(operation => new(operation)
{
    Summary = "Delete Item",
    Description = "Delete an item by entering itemId.",
})
.WithOpenApi(generatedOperation =>
{
    generatedOperation.Parameters[0].Description = "The ID of the item to delete.";
    return generatedOperation;
});

/////////////////////
// LOCATIONS START //
/////////////////////

app.MapPost("/Location", (LocationService locationService, string name, double longitude, double latitude) =>
{
    return locationService.CreateLocation(name, longitude, latitude);
}).WithTags(locationTag)
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
}).WithTags(locationTag)
.WithOpenApi(operation => new(operation)
{
    Summary = "Get All Locations",
    Description = "Retrieve a list of all locations.",
});

app.MapGet("/Location/Get/ById", (LocationService locationService, int locationId) =>
{
    return locationService.GetById(locationId);
}).WithTags(locationTag)
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
}).WithTags(locationTag)
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
}).WithTags(locationTag)
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


app.Run();