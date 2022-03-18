using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToyRobot.Model;
using ToyRobot.Services;
using ToyRobot.SqlServerModel.DB;

namespace ToyRobot.SqlServerModel.Services
{
    public class RobotSqlServerDBService : IRobotStepHistoryService
    {
        public const int DefaultMaxRobots = 5;

        private readonly ToyRobotDbContext _toyRobotDbContext;
        public Player? CurrentPlayer { get; private set; } = null;
        public Robot? CurrentRobot { get; private set; } = null;
        public Map? CurrentMap = null;
        public RobotSqlServerDBService(ToyRobotDbContext toyRobotDbContext)
        {
            this._toyRobotDbContext = toyRobotDbContext;
        }

        public async Task AddResizeMapStepAsync(int width, int height)
        {
            if(CurrentPlayer == null)
                throw new ArgumentNullException(nameof(CurrentPlayer));
            if (CurrentMap != null && CurrentMap.Width == width && CurrentMap.Height == height)
                return;
            var existingMap = await _toyRobotDbContext.Map.FirstOrDefaultAsync(m=> m.Width == width && m.Height == height && m.CreatedByPlayerId==CurrentPlayer.PlayerId);
            if(existingMap != null)
            {
                CurrentMap = existingMap;
            }
            else
            {
                var newMap = new Map { Width = width, Height = height, CreationDate = DateTime.UtcNow, CreatedByPlayerId = CurrentPlayer.PlayerId, MaxRobots = (CurrentMap?.MaxRobots ?? DefaultMaxRobots) };
                CurrentPlayer.Map.Add(newMap);
                await _toyRobotDbContext.SaveChangesAsync();
                CurrentMap = newMap;
                Debug.Assert(CurrentMap?.MapId > 0);
            }
        }

        public async Task AddStep(MapPosition? positionBeforeCommand, MapPosition? positionAfterCommand, string command, bool commandExecuted, string? result)
        {
            Debug.Assert(CurrentPlayer != null);
            Debug.Assert(CurrentRobot != null);
            if (commandExecuted)
            {
                int? orientationId = null;
                if (positionAfterCommand!=null && positionAfterCommand.Orientation!=MapOrientationEnum.NOT_SET)
                {
                    var orientation = await _toyRobotDbContext.Orientation.FirstOrDefaultAsync(t=>t.Name.Equals(positionAfterCommand.Orientation.ToString()));
                    orientationId = orientation?.OrientationId;
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
            }
        }

    }
}
