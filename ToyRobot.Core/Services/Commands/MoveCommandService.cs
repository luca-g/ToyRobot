using Microsoft.Extensions.Logging;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;
using ToyRobot.Core.Model;
using System.Resources;

namespace ToyRobot.Core.Services.Commands;

public class MoveCommandService : ICommand
{
    public string FirstInstruction => "MOVE";
    private readonly ILogger<MoveCommandService> loggerService;
    private readonly IRobotService robotService;
    private readonly IApplicationMessagesService applicationMessagesService;
    public CommandResultEnum CommandResult { get; set; }
    public string? ExecuteResultText { get; set; }

    public MoveCommandService(
        ILogger<MoveCommandService> logger,
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
                this.loggerService.LogTrace("MOVE command: active robot is null");
                applicationMessagesService.SetResult(this, CommandResultEnum.ActiveRobotNull);
                return false;
            }
            if (robot.Position == null)
            {
                this.loggerService.LogTrace("MOVE command: The robot is not in the map");
                applicationMessagesService.SetResult(this, CommandResultEnum.RobotPositionNull);
                return false;
            }
            var newPosition = robot.Position.Move();
            if (newPosition is not IMapPoint newPoint)
            {
                throw new InvalidCastException("Error converting map position to point");
            }
            if (!robot.Map.IsInMap(newPoint))
            {
                this.loggerService.LogTrace("MOVE command: The robot cannot move outside the map");
                applicationMessagesService.SetResult(this, CommandResultEnum.RobotCannotMoveOutsideMap);
                return true;
            }
            await this.robotService.SetMapPosition(robot, newPosition);
            this.loggerService.LogTrace("MOVE command: robot moved to position {X},{Y}", robot.Position.X, robot.Position.Y);
            applicationMessagesService.SetResult(this, CommandResultEnum.Ok);
            return true;
        }
        catch (Exception ex)
        {
            this.loggerService.LogError(ex, "MoveCommand unexpected error");
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
