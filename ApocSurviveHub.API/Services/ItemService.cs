using ApocSurviveHub.API.Models;
using ApocSurviveHub.API.Interfaces;

namespace ApocSurviveHub.API.Services;

public class ItemService
{

    private readonly ICrud<Item> _itemRepository;

    public ItemService(ICrud<Item> itemRepository)
    {
        _itemRepository = itemRepository;
    }
    public Item CreateItem(
            string name,
            string type,
            int? locationId)
    {
        var item = new Item(name, type, locationId);
        _itemRepository.Create(item);

        return item;
    }

    public Item GetById(int id)
    {
        return _itemRepository.GetById(id, i => i.Location, i => i.Location.Coordinates);
    }

    public IEnumerable<Item> GetItems()
    {
        return _itemRepository.GetAll(i => i.Location, i => i.Location.Coordinates);
    }

    public Item? UpdateItem(int itemId, string? name, string? type)
    {
        var item = _itemRepository.GetById(itemId);

        if (item is null) return null;

        item.Name = name ?? item.Name;
        item.Type = type ?? item.Type;

        _itemRepository.Update(item);
        return item;
    }

    public Item? DeleteItem(int itemId)
    {
        var item = _itemRepository.GetById(itemId);
        if (item is null) return null;

        _itemRepository.Delete(item);

        return item;
    }
}
