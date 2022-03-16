using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ToyRobot.Configuration;
using ToyRobot.Model;

namespace ToyRobot.Services;

public class MapService
{
    private readonly IOptions<MapSettings> _mapSettings;
    private readonly ILogger<MapService> _logger;
    public MapService(ILogger<MapService> logger, IOptions<MapSettings> mapSettings)
    {
        this._mapSettings = mapSettings;
        this._logger = logger;
    }
    public bool IsInMap(IMapPoint mapPoint)
    {
        if (mapPoint.X < 0 || mapPoint.X >= _mapSettings.Value.Width || mapPoint.Y < 0 || mapPoint.Y >= _mapSettings.Value.Height)
        {
            _logger.LogTrace("Point ({X},{Y}) is outside the map ({Width},{Height})", mapPoint.X, mapPoint.Y, _mapSettings.Value.Width, _mapSettings.Value.Height);
            return false;
        }
        _logger.LogTrace("Point ({X},{Y}) is inside the map ({Width},{Height})", mapPoint.X, mapPoint.Y, _mapSettings.Value.Width, _mapSettings.Value.Height);
        return true;
    }
}
