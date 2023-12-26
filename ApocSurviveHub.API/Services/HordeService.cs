using ApocSurviveHub.API.Models;
using ApocSurviveHub.API.Data;
using Microsoft.AspNetCore.Mvc;
using ApocSurviveHub.API.Interfaces;

namespace ApocSurviveHub.API.Services;

public class HordeService
{

    private readonly ICrud<Horde> _hordeRepository;

    public HordeService(ICrud<Horde> hordeRepository)
    {
        _hordeRepository = hordeRepository;
    }
    public IActionResult CreateHorde(
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

        return new CreatedResult($"/Horde/{horde.Id}", horde);
    }

    public IEnumerable<Horde> GetHordes()
    {
        return _hordeRepository.GetAll();
    }

    public Horde GetById(int id)
    {
        return _hordeRepository.GetById(id);
    }

    public IActionResult UpdateHorde(
              int id,
              string? Name,
              int? ThreatLevel,
              int? locationId)
    {
        var horde = _hordeRepository.GetById(id);
        if (horde is null) return new NotFoundResult();

        horde.Name = Name ?? horde.Name;
        horde.ThreatLevel = ThreatLevel ?? horde.ThreatLevel;

        if (locationId.HasValue)
        {
            horde.LocationId = locationId.Value;

        }

        _hordeRepository.Update(horde);
        return new OkObjectResult(horde);
    }

    public IActionResult DeleteHorde(int id)
    {
        var horde = _hordeRepository.GetById(id);
        if (horde is null) return new NotFoundResult();

        _hordeRepository.Delete(horde);

        return new OkObjectResult(horde);
    }
}
