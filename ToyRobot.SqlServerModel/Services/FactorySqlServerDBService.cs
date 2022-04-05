using Microsoft.Extensions.Logging;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;
using ToyRobot.SqlServerModel.Model;

namespace ToyRobot.SqlServerModel.Services;

public class FactorySqlServerDBService : IFactoryService
{
    private readonly ILogger<FactorySqlServerDBService> logger;
    private readonly IPlayerService playerService;
    private readonly IMapService mapService;
    private readonly IRobotService robotService;
    public FactorySqlServerDBService(
        ILogger<FactorySqlServerDBService> logger, 
        IPlayerService playerService, 
        IMapService mapService, 
        IRobotService robotService)
    {
        this.logger = logger;
        this.playerService = playerService;
        this.mapService = mapService;
        this.robotService = robotService;
    }
    public async Task<IScenario> CreateScenario()
    {
        return await CreateScenario(null, null, null);
    }
    private async Task<IPlayer> CreateOrLoadPlayer(Guid? playerGuid)
    {
        try
        {
            logger.LogTrace("CreateOrLoadPlayer Guid {playerGuid}", playerGuid);
            IPlayer? player = null;
            if (playerGuid == null)
            {
                player = await this.playerService.CreatePlayer();
            }
            else
            {
                player = await playerService.LoadPlayer(playerGuid.Value);
            }
            if (player == null)
            {
                throw new Exception("player is null");
            }
            logger.LogTrace("CreateOrLoadPlayer completed PlayerId {playerId}", player.PlayerId);
            return player;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating IPlayer");
            throw;
        }
    }
    private async Task<IMap> CreateOrLoadMap(int playerId, int? mapId)
    {
        try
        {
            logger.LogTrace("CreateOrLoadPlayer  player {playerId}, map {mapId}", playerId, mapId);

            IMap? map = null;
            if (mapId == null)
            {
                map = await this.mapService.CreateMap(playerId, mapService.MapSettings.MinWidth, mapService.MapSettings.MinHeight);
            }
            else
            {
                var maps = await this.mapService.LoadMaps(playerId);
                map = maps.FirstOrDefault(t => t.MapId == mapId.Value);
            }
            if (map == null)
            {
                throw new Exception("map is null");
            }
            logger.LogTrace("CreateOrLoadPlayer completed player {playerId}, map {mapId}", playerId, map.MapId);
            return map;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating IMap");
            throw;
        }
    }
    private async Task<IRobot> CreateOrLoadRobot(int playerId, int mapId, int? robotId)
    {
        try
        {
            logger.LogTrace("CreateOrLoadRobot  player {playerId}, map {mapId}, robot {robotId}", playerId, mapId, robotId);

            IRobot? robot = null;
            if (robotId == null)
            {
                robot = await this.robotService.CreateRobot(playerId, mapId);
            }
            else
            {
                var robots = await this.robotService.LoadRobots(playerId, mapId);
                if (robots != null && robots.Count == 1)
                {
                    robot = robots[0];
                }
            }
            if (robot == null)
            {
                throw new Exception("robot is null");
            }
            logger.LogTrace("CreateOrLoadRobot completed player {playerId}, map {mapId}, robot {robotId}", playerId, mapId, robot.RobotId);
            return robot;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating IRobot");
            throw;
        }
    }
    public async Task<IScenario> CreateScenario(Guid? playerId, int? mapId, int? robotId)
    {
        if(await CreateOrLoadPlayer(playerId) is not DB.Player player)
        {
            throw new Exception("player is null");
        }

        if (await CreateOrLoadMap(player.PlayerId, mapId) is not DB.Map map)
        {
            throw new Exception("map is null");
        }

        if (await CreateOrLoadRobot(player.PlayerId, map.MapId, robotId) is not DB.Robot robot)
        {
            throw new Exception("robot is null");
        }

        var scenario = new ScenarioSqlServerDB(player, map, robot);
        return scenario;
    }
}
