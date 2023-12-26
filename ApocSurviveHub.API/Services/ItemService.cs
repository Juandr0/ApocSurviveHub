using ApocSurviveHub.API.Models;
using Microsoft.AspNetCore.Mvc;
using ApocSurviveHub.API.Interfaces;

namespace ApocSurviveHub.API.Services;

public class ItemService
{

    private readonly ICrud<Item> _itemRepository;

    public ItemService(ICrud<Item> itemRepository)
    {
        _itemRepository = itemRepository;
    }
    public IActionResult CreateItem(
            string name,
            string type,
            int? locationId)
    {
        var item = new Item(name, type, locationId);
        _itemRepository.Create(item);

        return new CreatedResult($"/Item/{item.Id}", item);
    }

    public Item GetById(int id)
    {
        return _itemRepository.GetById(id, i => i.Location, i => i.Location.Coordinates);
    }

    public IEnumerable<Item> GetItems()
    {
        return _itemRepository.GetAll(i => i.Location, i => i.Location.Coordinates);
    }

    public IActionResult UpdateItem(int itemId, string? name, string? type)
    {
        var item = _itemRepository.GetById(itemId);

        if (item is null) return new NotFoundResult();

        item.Name = name ?? item.Name;
        item.Type = type ?? item.Type;

        _itemRepository.Update(item);
        return new OkObjectResult(item);
    }

    public IActionResult DeleteItem(int itemId)
    {
        var item = _itemRepository.GetById(itemId);
        if (item is null) return new NotFoundResult();

        _itemRepository.Delete(item);

        return new OkObjectResult(item);
    }
}
