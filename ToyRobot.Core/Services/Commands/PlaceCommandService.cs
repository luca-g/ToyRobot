using Microsoft.Extensions.Logging;
using System.Diagnostics;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;
using ToyRobot.Core.Model;

namespace ToyRobot.Core.Services.Commands;

public class PlaceCommandService : ICommand
{
    public string FirstInstruction => "PLACE";
    public string ConsoleInstruction { get => "PLACE x,y,direction (NORTH,EAST,SOUTH,WEST)"; }
    public string? ExecuteResultText { get; set; }
    public CommandResultEnum CommandResult { get; set; }

    private readonly ILogger<PlaceCommandService> loggerService;
    private readonly IApplicationMessagesService applicationMessagesService;
    private string[]? commandParts=null;
    
    private int x;
    private int y;
    private MapOrientationEnum mapOrientation;
    
    public PlaceCommandService(
        ILogger<PlaceCommandService> logger, 
        IApplicationMessagesService applicationMessagesService)
    {
        this.loggerService = logger;
        this.applicationMessagesService = applicationMessagesService;
    }
    public async Task<bool> Execute(IScenario scenario)
    {
        try
        {
            Debug.Assert(commandParts != null);
            Debug.Assert(commandParts.Length == 4);
            if (!scenario.IsRobotSet)
            {
                this.loggerService.LogTrace("PLACE command: active robot is null");
                applicationMessagesService.SetResult(scenario.Language, this, CommandResultEnum.ActiveRobotNull);
                return false;
            }
            var mapPositon = new MapPosition(x, y, mapOrientation);
            if (!scenario.IsInMap(mapPositon))
            {
                this.loggerService.LogTrace("PLACE position set outside the map");
                applicationMessagesService.SetResult(scenario.Language, this, CommandResultEnum.RobotCannotMoveOutsideMap);
                return false;
            }
            await scenario.SetMapPosition(mapPositon);
            this.loggerService.LogTrace("PLACE command: robot at position {x},{y}", x, y);
            applicationMessagesService.SetResult(scenario.Language, this, CommandResultEnum.Ok);
            return true;
        }
        catch (Exception ex)
        {
            this.loggerService.LogError(ex, "PLACE Command unexpected error");
            throw;
        }
    }

    public bool TryParse(string[] commandParts)
    {
        if (commandParts.Length != 4)
            return false;
        if (FirstInstruction.Equals(commandParts[0]))
        {
            if (!int.TryParse(commandParts[1], out x))
            {
                this.loggerService.LogTrace("PLACE invalid X value: {x}", commandParts[1]);
                return false;
            }
            if (!int.TryParse(commandParts[2], out y))
            {
                this.loggerService.LogTrace("PLACE invalid Y value: {y}", commandParts[2]);
                return false;
            }
            if (!Enum.TryParse(commandParts[3], out mapOrientation))
            {
                this.loggerService.LogTrace("PLACE invalid orientation value: {orientation}", commandParts[3]);
                return false;
            }
            this.commandParts = commandParts;
            return true;
        }
        return false;
    }
}

