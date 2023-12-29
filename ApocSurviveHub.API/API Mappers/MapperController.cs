using ApocSurviveHub.API.Mappers;

abstract class MapperController()
{
    public static void MapEndpoints(WebApplication app)
    {
        SurvivorMapper.MapSurvivorActions(app);
        HordeMapper.MapHordeActions(app);
        ItemMapper.MapItemActions(app);
        LocationMapper.MapLocationActions(app);
    }
}