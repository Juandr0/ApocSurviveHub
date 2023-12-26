using ApocSurviveHub.API.Models;
using ApocSurviveHub.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApocSurviveHub.API.Interfaces;

namespace ApocSurviveHub.API.Services;

public class SurvivorService
{
    private readonly ICrud<Survivor> _survivorRepository;
    private readonly ICrud<Item> _itemRepository;

    public SurvivorService(ICrud<Survivor> survivorRepository, ICrud<Item> itemRepository)
    {
        _survivorRepository = survivorRepository;
        _itemRepository = itemRepository;
    }
    public IActionResult CreateSurvivor(
    string Name,
    bool IsAlive,
    int? locationId)
    {
        var survivor = new Survivor(Name, IsAlive, locationId);

        if (locationId.HasValue)
        {
            survivor.LocationId = locationId.Value;
        }

        _survivorRepository.Create(survivor);

        return new CreatedResult($"/Survivor/{survivor.Id}", survivor);
    }
    public IEnumerable<Survivor> GetSurvivors()
    {
        return _survivorRepository.GetAll(s => s.Inventory, s => s.Location, s => s.Location.Coordinates);
    }

    public IActionResult UpdateSurvivor(
        int id,
        string? Name,
        bool? IsAlive,
        int? locationId)
    {
        // var survivor = await dbContext.Survivors
        //     .Include(s => s.Inventory)
        //     .Include(s => s.Location)
        //     .FirstOrDefaultAsync(s => s.Id == survivorId);
        var survivor = _survivorRepository.GetById(id);
        if (survivor is null) return new NotFoundResult();

        survivor.Name = Name ?? survivor.Name;
        survivor.IsAlive = IsAlive ?? survivor.IsAlive;

        if (locationId.HasValue)
        {
            survivor.LocationId = locationId;
            foreach (var item in survivor.Inventory)
            {
                item.LocationId = locationId;
            }
        }

        return new OkObjectResult(survivor);
    }

    public IActionResult DeleteSurvivor(int survivorId)
    {
        var survivor = _survivorRepository.GetById(survivorId);
        if (survivor is null) return new NotFoundResult();

        _survivorRepository.Delete(survivor);
        return new OkObjectResult(survivor);
    }

    // Survivor item handling

    public IActionResult AddItem(int survivorId, int itemId)
    {
        var survivor = _survivorRepository.GetById(survivorId);
        if (survivor is null) return new NotFoundResult();

        var item = _itemRepository.GetById(itemId);
        if (item is null) return new NotFoundResult();

        if (survivor.LocationId is null)
        {
            survivor.Inventory.Add(item);
            _survivorRepository.Update(survivor);
            return new OkObjectResult(survivor);
        }

        if (item.LocationId is not null)
        {
            if (item.LocationId == survivor.LocationId)
            {
                item.LocationId = survivor.LocationId;
                item.Location = survivor.Location;
            }
            else
            {
                Console.WriteLine("Survivor and Item is not at the same place");
            }
        }
        else
        {
            item.LocationId = survivor.LocationId;
            item.Location = survivor.Location;
            _survivorRepository.Update(survivor);
        }

        return new OkObjectResult(survivor);
    }

    public IActionResult RemoveItem(int survivorId, int itemId)
    {
        var survivor = _survivorRepository.GetById(survivorId);
        if (survivor is null) return new NotFoundResult();

        var item = _itemRepository.GetById(itemId);
        if (item is null) return new NotFoundResult();
        survivor.Inventory.Remove(item);
        _survivorRepository.Update(survivor);

        return new OkObjectResult(survivor);
    }
}

