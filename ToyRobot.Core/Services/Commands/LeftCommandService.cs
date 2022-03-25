using Microsoft.Extensions.Logging;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;

namespace ToyRobot.Core.Services.Commands;

public class LeftCommandService : ICommand
{
    public string FirstInstruction => "LEFT";
    private readonly ILogger<LeftCommandService> loggerService;
    private readonly IRobotService robotService;
    public LeftCommandService(
        ILogger<LeftCommandService> logger,
        IRobotService robotService
        )
    {
        this.loggerService = logger;
        this.robotService = robotService;
    }
    public async Task<bool> Execute()
    {
        var robot = robotService.ActiveRobot;
        try
        {
            if (robot == null)
            {
                this.loggerService.LogTrace("LEFT command: active robot is null");
                return false;
            }
            if (robot.Map == null)
            {
                this.loggerService.LogTrace("LEFT command: the map is not selected");
                return false;
            }
            if (robot.Position == null)
            {
                this.loggerService.LogTrace("LEFT command: The robot is not in the map");
                return false;
            }
            var newPosition = robot.Position.Left();
            await this.robotService.SetMapPosition(robot, newPosition);
            this.loggerService.LogTrace("LEFT command: robot moved to position {Orientation}", newPosition.Orientation);
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

