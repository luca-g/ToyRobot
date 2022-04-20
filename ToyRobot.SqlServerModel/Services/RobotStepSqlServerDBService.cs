using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;
using ToyRobot.SqlServerModel.DB;

namespace ToyRobot.SqlServerModel.Services
{
    public class RobotStepSqlServerDBService : IRobotStepHistoryService
    {
        public const int DefaultMaxRobots = 5;

        private readonly ToyRobotDbContext toyRobotDbContext;
        private readonly ILogger<RobotStepSqlServerDBService> logger;

        public RobotStepSqlServerDBService(
            ILogger<RobotStepSqlServerDBService> logger, 
            ToyRobotDbContext toyRobotDbContext
            )
        {
            this.toyRobotDbContext = toyRobotDbContext;
            this.logger = logger;
        }

        public async Task AddStep(IScenario scenario, string command, bool commandExecuted)
        {
            try
            { 
                if (commandExecuted)
                {
                    var positionAfterCommand = scenario.RobotPosition;
                    int? orientationId = null;
                    if (positionAfterCommand!=null)
                    {
                        orientationId = (int)positionAfterCommand.Orientation;
                    }
                    var commandObj = new Command { 
                        CommandDate = DateTime.UtcNow, 
                        CommandText = command,  
                        OrientationId = orientationId, 
                        RobotId = scenario.RobotId,
                        X = positionAfterCommand?.X,
                        Y = positionAfterCommand?.Y,                    
                    };
                    toyRobotDbContext.Command.Add(commandObj);
                    await toyRobotDbContext.SaveChangesAsync();
                    logger.LogTrace("Command saved {command}", command);
                }
                else
                    logger.LogTrace("Command not saved because it was not executed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error");
                throw;
            }
        }

    }
}
