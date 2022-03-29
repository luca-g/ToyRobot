using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;
using ToyRobot.SqlServerModel.DB;

namespace ToyRobot.SqlServerModel.Services;

public class RobotSqlServerDBService : IRobotService
{
    private readonly ILogger<RobotSqlServerDBService> loggerService;
    private readonly ToyRobotDbContext toyRobotDbContext;

    public RobotSqlServerDBService(
        ILogger<RobotSqlServerDBService> logger, 
        ToyRobotDbContext toyRobotDbContext
        )
    {
        this.loggerService = logger;
        this.toyRobotDbContext = toyRobotDbContext;
    }

    public IRobot? ActiveRobot { get; set; }

    public async Task<IRobot> CreateRobot(int playerId, int mapId)
    {
        Debug.Assert(playerId > 0);
        Debug.Assert(mapId > 0);
        try
        {
            loggerService.LogTrace("Creating robot for player {playerId} in map {mapId}", playerId, mapId);
            var robot = new Robot
            {
                CreationDate = DateTime.UtcNow,
                PlayerId = playerId,
                MapId = mapId
            };
            toyRobotDbContext.Robot.Add(robot);
            await toyRobotDbContext.SaveChangesAsync();
            Debug.Assert(robot.RobotId > 0);
            loggerService.LogTrace("Created robot {RobotId} for player {playerId} in map {mapId}", robot.RobotId, playerId, mapId);
            return (IRobot)robot;
        }
        catch (Exception ex)
        {
            loggerService.LogError(ex, "Error creating robot for player {playerId} in map {mapId}", playerId, mapId);
            throw;
        }
    }

    public async Task<IList<IRobot>> LoadRobots(int playerId, int? mapId)
    {
        Debug.Assert(playerId > 0);
        try
        {
            loggerService.LogTrace("Loading robots for player {playerId} and map {mapId}", playerId, mapId);
            var outputParam = new OutputParameter<int>();
            var robots = await this.toyRobotDbContext.Procedures.LoadRobotsAsync(playerId, null, mapId, outputParam);
            if (robots == null)
            {
                return new List<IRobot>();
            }
            var result = robots
                .Select(t => new Robot { PlayerId = t.PlayerId, MapId = t.MapId, X=t.X, Y=t.Y, OrientationId=t.OrientationId, CreationDate = t.CreationDate } as IRobot)
                .ToList();
            loggerService.LogTrace("Loadied robots for player {playerId} and map {mapId}", playerId, mapId);
            return result;
        }
        catch (Exception ex)
        {
            loggerService.LogError(ex, "Error loading robots");
            throw;
        }
    }

    public async Task SetMapPosition(IRobot robot, IMapPosition? mapPosition)
    {
        try {
            if (robot == null) throw new ArgumentNullException(nameof(robot));
            loggerService.LogTrace("Setting map posiiton player {PlayerId}, robot {RobotId}", robot.Player.PlayerId, robot.RobotId);

            Robot? ctxRobot;
            if(robot is Robot robotObject)
            {
                ctxRobot = robotObject;
            }
            else
            {
                ctxRobot = await toyRobotDbContext.Robot.SingleOrDefaultAsync(t => t.RobotId == robot.RobotId);
                if (ctxRobot == null)
                {
                    loggerService.LogTrace("Robot not found in the database");
                    return;
                }
            }

            if (mapPosition == null)
            {
                ctxRobot.X = null;
                ctxRobot.Y = null;
                ctxRobot.OrientationId = null;
            }
            else
            {
                ctxRobot.X = mapPosition.X;
                ctxRobot.Y = mapPosition.Y;
                ctxRobot.OrientationId = (int)mapPosition.Orientation;
            }
            await robot.SetMapPosition(mapPosition);
            await toyRobotDbContext.SaveChangesAsync();
            loggerService.LogTrace("Robot map posiiton set player {PlayerId}, robot {RobotId},position {mapPosition}",
                robot.Player.PlayerId, robot.RobotId, mapPosition?.ToString()??"null");
        }
        catch (Exception ex)
        {
            loggerService.LogError(ex, "Error SetMapPosition");
            throw;
        }
    }
}
