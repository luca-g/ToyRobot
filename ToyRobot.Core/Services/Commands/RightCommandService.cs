﻿using Microsoft.Extensions.Logging;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;

namespace ToyRobot.Core.Services.Commands;

public class RightCommandService : ICommand
{
    public string FirstInstruction => "RIGHT";

    public string? ExecuteResultText { get; set; }
    public CommandResultEnum CommandResult { get; set; }

    private readonly ILogger<RightCommandService> loggerService;
    private readonly IRobotService robotService;
    private readonly IApplicationMessagesService applicationMessagesService;
    public RightCommandService(
        ILogger<RightCommandService> logger, 
        IRobotService robotService,
        IApplicationMessagesService applicationMessagesService
        )
    {
        this.loggerService = logger;
        this.robotService = robotService;
        this.applicationMessagesService = applicationMessagesService;
    }
    public async Task<bool> Execute()
    {
        var robot = robotService.ActiveRobot;
        try
        {
            if (robot == null)
            {
                this.loggerService.LogTrace("RIGHT command: active robot is null");
                applicationMessagesService.SetResult(this, CommandResultEnum.ActiveRobotNull);
                return false;
            }
            if (robot.Position == null)
            {
                this.loggerService.LogTrace("RIGHT command: The robot is not in the map");
                applicationMessagesService.SetResult(this, CommandResultEnum.RobotPositionNull);
                return false;
            }
            var newPosition = robot.Position.Right();
            await this.robotService.SetMapPosition(robot, newPosition);
            this.loggerService.LogTrace("RIGHT command: robot moved to position {Orientation}", newPosition.Orientation);
            applicationMessagesService.SetResult(this, CommandResultEnum.Ok);
            return true;
        }
        catch (Exception ex)
        {
            this.loggerService.LogError(ex, "RIGHT Command unexpected error");
            throw;
        }
    }

    public bool TryParse(string[] commandParts)
    {
        if (commandParts.Length != 1)
            return false;
        return FirstInstruction.Equals(commandParts[0]);
    }
}

