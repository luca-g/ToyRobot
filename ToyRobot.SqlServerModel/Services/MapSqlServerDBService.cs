﻿using Microsoft.Extensions.Logging;
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
    public MapSqlServerDBService(
        ILogger<MapSqlServerDBService> logger, 
        ToyRobotDbContext toyRobotDbContext, 
        IOptions<MapSettings> mapSettings)
    {
        this.loggerService = logger;
        this.toyRobotDbContext = toyRobotDbContext;
        this.MapSettings = mapSettings.Value;
    }
    public MapSettings MapSettings { get; private set; }

    public IMap? LoadMap(int mapId)
    {
        try
        {
            loggerService.LogTrace("Load single map by id {mapId}", mapId);
            var outputParam = new OutputParameter<int>();
            var map = toyRobotDbContext.Map.SingleOrDefault(t => t.MapId == mapId);                
            loggerService.LogTrace("AvailableMaps: Loaded maps, maps found: {Count}", map==null?0:1);
            return map;
        }
        catch (Exception ex)
        {
            loggerService.LogError(ex, "Error loading maps");
            throw;
        }
    }

    public async Task<IList<IMap>> LoadMaps(int playerId)
    {
        Debug.Assert(playerId > 0);
        try
        {
            loggerService.LogTrace("AvailableMaps: Loading maps for player {playerId}", playerId);
            var outputParam = new OutputParameter<int>();
            var maps = await toyRobotDbContext.Procedures.LoadMapsAsync(playerId, outputParam);
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

    public async Task<IMap> CreateMap(int playerId, int width, int heigth)
    {
        try
        {
            loggerService.LogTrace("CreateMap player {playerId}", playerId);
            if (width <= 0)
            {
                throw new ArgumentException("Map width cannot be lower than 1");
            }
            if (heigth <= 0)
            {
                throw new ArgumentException("Map heigth cannot be lower than 1");
            }
            var map = new Map
            {
                Width = width,
                Height = heigth,
                CreationDate = DateTime.UtcNow,
            };
            toyRobotDbContext.Map.Add(map);
            await toyRobotDbContext.SaveChangesAsync();
            Debug.Assert(map.MapId > 0);
            loggerService.LogTrace("CreateMap: map created - player {playerId}, map {MapId}", playerId, map.MapId);
            return map;
        }
        catch (Exception ex)
        {
            loggerService.LogError(ex, "Error creating map");
            throw;
        }
    }

}
