using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;
using ToyRobot.SqlServerModel.DB;

namespace ToyRobot.SqlServerModel.Services
{
    public class RobotSqlServerDBService : IRobotStepHistoryService
    {
        public const int DefaultMaxRobots = 5;

        private readonly ToyRobotDbContext _toyRobotDbContext;
        private readonly ILogger<RobotSqlServerDBService> _logger;

        public Player? CurrentPlayer { get; private set; } = null;
        public Robot? CurrentRobot { get; private set; } = null;
        public Map? CurrentMap = null;
        public RobotSqlServerDBService(ILogger<RobotSqlServerDBService> logger, ToyRobotDbContext toyRobotDbContext)
        {
            this._toyRobotDbContext = toyRobotDbContext;
            this._logger = logger;
        }

        public async Task AddResizeMapStepAsync(int width, int height)
        {
            Debug.Assert(CurrentPlayer != null);
            try
            {
                if (CurrentMap != null && CurrentMap.Width == width && CurrentMap.Height == height)
                {
                    _logger.LogTrace("AddResizeMapStepAsync: map unchanged, same size");
                    return;
                }
                var existingMap = await _toyRobotDbContext.Map.FirstOrDefaultAsync(m => m.Width == width && m.Height == height && m.CreatedByPlayerId == CurrentPlayer.PlayerId);
                if (existingMap != null)
                {
                    CurrentMap = existingMap;
                    _logger.LogTrace("AddResizeMapStepAsync: existing map selected: id {0}, w {1}, h {2}", CurrentMap.MapId, CurrentMap.Width, CurrentMap.Height);
                }
                else
                {
                    var newMap = new Map { Width = width, Height = height, CreationDate = DateTime.UtcNow, CreatedByPlayerId = CurrentPlayer.PlayerId, MaxRobots = (CurrentMap?.MaxRobots ?? DefaultMaxRobots) };
                    CurrentPlayer.Map.Add(newMap);
                    await _toyRobotDbContext.SaveChangesAsync();
                    CurrentMap = newMap;
                    _logger.LogTrace("AddResizeMapStepAsync: new map created: id {0}, w {1}, h {2}", CurrentMap.MapId, CurrentMap.Width, CurrentMap.Height);
                    Debug.Assert(CurrentMap?.MapId > 0);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error");
                throw;
            }
        }

        public async Task AddStep(IMapPosition? positionBeforeCommand, IMapPosition? positionAfterCommand, string command, bool commandExecuted, string? result)
        {
            Debug.Assert(CurrentPlayer != null);
            Debug.Assert(CurrentRobot != null);
            try
            { 
                if (commandExecuted)
                {
                    int? orientationId = null;
                    if (positionAfterCommand!=null && positionAfterCommand.Orientation!=MapOrientationEnum.NOT_SET)
                    {
                        var orientation = await _toyRobotDbContext.Orientation.FirstOrDefaultAsync(t=>t.Name.Equals(positionAfterCommand.Orientation.ToString()));
                        orientationId = orientation?.OrientationId;
                        Debug.Assert(orientationId != null);
                    }
                    var commandObj = new Command { 
                        CommandDate = DateTime.UtcNow, 
                        CommandText = command,  
                        OrientationId = orientationId, 
                        RobotId = CurrentRobot.RobotId,
                        X = positionAfterCommand?.X,
                        Y = positionAfterCommand?.Y,                    
                    };
                    _toyRobotDbContext.Command.Add(commandObj);
                    await _toyRobotDbContext.SaveChangesAsync();
                    _logger.LogTrace("Command saved {0}", command);
                }
                else
                    _logger.LogTrace("Command not saved because it was not executed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error");
                throw;
            }
        }

    }
}
