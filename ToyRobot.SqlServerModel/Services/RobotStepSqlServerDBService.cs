﻿using System.Diagnostics;
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

        private readonly ToyRobotDbContext _toyRobotDbContext;
        private readonly ILogger<RobotStepSqlServerDBService> _logger;
        private readonly IRobotService robotService;

        public RobotStepSqlServerDBService(
            ILogger<RobotStepSqlServerDBService> logger, 
            ToyRobotDbContext toyRobotDbContext,
            IRobotService robotService
            )
        {
            this._toyRobotDbContext = toyRobotDbContext;
            this._logger = logger;
            this.robotService = robotService;
        }

        public async Task AddStep(IMapPosition? positionBeforeCommand, IMapPosition? positionAfterCommand, string command, bool commandExecuted, string? result)
        {
            try
            { 
                if (commandExecuted)
                {
                    int? orientationId = null;
                    if (positionAfterCommand!=null && positionAfterCommand.Orientation!=MapOrientationEnum.NOT_SET)
                    {
                        orientationId = (int)positionAfterCommand.Orientation;
                    }
                    var commandObj = new Command { 
                        CommandDate = DateTime.UtcNow, 
                        CommandText = command,  
                        OrientationId = orientationId, 
                        RobotId = robotService.ActiveRobot?.RobotId,
                        X = positionAfterCommand?.X,
                        Y = positionAfterCommand?.Y,                    
                    };
                    _toyRobotDbContext.Command.Add(commandObj);
                    await _toyRobotDbContext.SaveChangesAsync();
                    _logger.LogTrace("Command saved {command}", command);
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
