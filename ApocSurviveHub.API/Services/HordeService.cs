using ApocSurviveHub.API.Models;
using ApocSurviveHub.API.Interfaces;
using SQLitePCL;

namespace ApocSurviveHub.API.Services;

public class HordeService
{

    private readonly ICrud<Horde> _hordeRepository;

    public HordeService(ICrud<Horde> hordeRepository)
    {
        _hordeRepository = hordeRepository;
    }
    public Horde CreateHorde(
        string Name,
        int ThreatLevel,
        int? locationId)
    {
        var horde = new Horde(Name, ThreatLevel, locationId);

        if (locationId.HasValue)
        {
            horde.LocationId = locationId.Value;
        }

        _hordeRepository.Create(horde);

        return horde;
    }

    public IEnumerable<Horde> GetHordes()
    {
        return _hordeRepository.GetAll(h => h.Location, h => h.Location.Coordinates);
    }

    public Horde GetById(int id)
    {
        return _hordeRepository.GetById(id, h => h.Location, h => h.Location.Coordinates);
    }

    public Horde? UpdateHorde(
              int id,
              string? Name,
              int? ThreatLevel,
              int? locationId)
    {
        var horde = _hordeRepository.GetById(id);
        if (horde is null) return null;

        horde.Name = Name ?? horde.Name;
        horde.ThreatLevel = ThreatLevel ?? horde.ThreatLevel;

        if (locationId.HasValue)
        {
            horde.LocationId = locationId.Value;
        }

        _hordeRepository.Update(horde);
        return horde;
    }

    public Horde? DeleteHorde(int id)
    {
        var horde = _hordeRepository.GetById(id);
        if (horde is null) return null;

        _hordeRepository.Delete(horde);

        return horde;
    }
}
