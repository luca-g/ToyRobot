using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;
using ToyRobot.Core.Configuration;
using ToyRobot.SqlServerModel.DB;

namespace ToyRobot.SqlServerModel.Services;

public class MapSqlServerDBService : IMapService
{
    private readonly ILogger<MapSqlServerDBService> loggerService;
    private readonly ToyRobotDbContext toyRobotDbContext;
    private readonly IPlayerService playerService;
    public MapSqlServerDBService(
        ILogger<MapSqlServerDBService> logger, 
        ToyRobotDbContext toyRobotDbContext, 
        IOptions<MapSettings> mapSettings,
        IPlayerService playerService)
    {
        this.loggerService = logger;
        this.toyRobotDbContext = toyRobotDbContext;
        this.MapSettings = mapSettings.Value;
        this.playerService = playerService;
    }
    public MapSettings MapSettings { get; private set; }
    public IMap? ActiveMap { get; set; }

    public async Task<IList<IMap>> LoadMaps(int playerId)
    {
        Debug.Assert(playerId > 0);
        try
        {
            loggerService.LogTrace("AvailableMaps: Loading maps for player {playerId}", playerId);
            var maps = await toyRobotDbContext.Procedures.LoadMapsAsync(playerId);
            if(maps == null)
            {
                return new List<IMap>();
            }
            var result = maps
                .Select(t=>new Map { MapId = t.MapId, Width = t.Width, Height = t.Height, CreationDate = t.CreationDate } as IMap)
                .ToList();
            loggerService.LogTrace("AvailableMaps: Loaded maps for player {playerId}, maps found: {Count}", playerId, maps.Count);
            return result;
        }
        catch (Exception ex)
        {
            loggerService.LogError(ex, "Error loading maps");
            throw;
        }
    }

    public async Task<IMap> CreateMap(int width, int heigth)
    {
        try
        {
            loggerService.LogTrace("CreateMap looking for existing map of same size - player {playerId}", playerService.ActivePlayer?.PlayerId);
            var map = new Map
            {
                Width = width,
                Height = heigth,
                CreationDate = DateTime.UtcNow,
            };
            toyRobotDbContext.Map.Add(map);
            await toyRobotDbContext.SaveChangesAsync();
            Debug.Assert(map.MapId > 0);
            this.ActiveMap = map;
            loggerService.LogTrace("CreateMap: map created - player {playerId}, map {MapId}", playerService.ActivePlayer?.PlayerId, map.MapId);
            return map;
        }
        catch (Exception ex)
        {
            loggerService.LogError(ex, "Error creating map");
            throw;
        }
    }

}
