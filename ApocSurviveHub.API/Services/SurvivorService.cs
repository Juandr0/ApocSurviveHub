using ApocSurviveHub.API.Models;
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
    public Survivor CreateSurvivor(
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

        return survivor;
    }
    public IEnumerable<Survivor> GetSurvivors()
    {
        return _survivorRepository.GetAll(s => s.Inventory, s => s.Location, s => s.Location.Coordinates);
    }

    public Survivor GetById(int survivorId)
    {
        return _survivorRepository.GetById(survivorId, s => s.Inventory, s => s.Location, s => s.Location.Coordinates);
    }

    public Survivor? UpdateSurvivor(
        int id,
        string? Name,
        bool? IsAlive,
        int? locationId)
    {
        var survivor = _survivorRepository.GetById(id, s => s.Inventory, s => s.Location);
        if (survivor is null) return null;

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

        return survivor;
    }

    public Survivor? DeleteSurvivor(int survivorId)
    {
        var survivor = _survivorRepository.GetById(survivorId);
        if (survivor is null) return null;

        _survivorRepository.Delete(survivor);
        return survivor;
    }

    // Survivor item handling

    public Survivor? AddItem(int survivorId, int itemId)
    {
        var survivor = _survivorRepository.GetById(survivorId);
        if (survivor is null) return null;

        var item = _itemRepository.GetById(itemId);
        if (item is null) return null;

        if (survivor.LocationId is null)
        {
            survivor.Inventory.Add(item);
            _survivorRepository.Update(survivor);
            return survivor;
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

        return null;
    }

    public Survivor? RemoveItem(int survivorId, int itemId)
    {
        var survivor = _survivorRepository.GetById(survivorId);
        if (survivor is null) return null;

        var item = _itemRepository.GetById(itemId);
        if (item is null) return null;
        survivor.Inventory.Remove(item);
        _survivorRepository.Update(survivor);

        return survivor;
    }
}

