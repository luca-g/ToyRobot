using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ToyRobot.Configuration;
using ToyRobot.Model;

namespace ToyRobot.Services;

public class MapService
{
    private readonly MapSettings _mapSettings;
    private readonly ILogger<MapService> _logger;
    public int Width => this._mapSettings.Width;
    public int Height => this._mapSettings.Height;
    public MapService(ILogger<MapService> logger, IOptions<MapSettings> mapSettings)
    {
        this._mapSettings = mapSettings.Value;
        this._logger = logger;
    }
    public void SetMapSize(int width, int height)
    {
        this._mapSettings.Width = width;
        this._mapSettings.Height = height;
    }

    public bool IsInMap(IMapPoint mapPoint)
    {
        if (mapPoint.X < 0 || mapPoint.X >= this.Width || mapPoint.Y < 0 || mapPoint.Y >= this.Height)
        {
            _logger.LogTrace("Point ({X},{Y}) is outside the map ({Width},{Height})", mapPoint.X, mapPoint.Y, this.Width, this.Height);
            return false;
        }
        _logger.LogTrace("Point ({X},{Y}) is inside the map ({Width},{Height})", mapPoint.X, mapPoint.Y, this.Width, this.Height);
        return true;
    }
}
