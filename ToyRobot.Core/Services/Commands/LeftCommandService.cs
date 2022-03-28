using Microsoft.Extensions.Logging;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;

namespace ToyRobot.Core.Services.Commands;

public class LeftCommandService : ICommand
{
    public string FirstInstruction => "LEFT";

    public string? ExecuteResultText {get; set;}
    public CommandResultEnum CommandResult {get; set;}

    private readonly ILogger<LeftCommandService> loggerService;
    private readonly IRobotService robotService;
    private readonly IApplicationMessagesService applicationMessagesService;
    public LeftCommandService(
        ILogger<LeftCommandService> logger,
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
                applicationMessagesService.SetResult(this, CommandResultEnum.ActiveRobotNull);
                this.loggerService.LogTrace("LEFT command: active robot is null");
                return false;
            }
            if (robot.Position == null)
            {
                applicationMessagesService.SetResult(this, CommandResultEnum.RobotPositionNull);
                this.loggerService.LogTrace("LEFT command: The robot is not in the map");
                return false;
            }
            var newPosition = robot.Position.Left();
            await this.robotService.SetMapPosition(robot, newPosition);
            this.loggerService.LogTrace("LEFT command: robot moved to position {Orientation}", newPosition.Orientation);
            applicationMessagesService.SetResult(this, CommandResultEnum.Ok);
            return true;
        }
        catch (Exception ex)
        {
            this.loggerService.LogError(ex, "LEFT Command unexpected error");
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

