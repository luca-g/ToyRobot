using Microsoft.Extensions.Logging;
using System.Diagnostics;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;

namespace ToyRobot.Core.Services.Commands;

public class CreateRobotCommandService : ICommand
{
    public string FirstInstruction => "CREATEROBOT";
    public string ConsoleInstruction { get => "CREATEROBOT"; }
    private readonly ILogger<CreateRobotCommandService> loggerService;
    private readonly IMapService mapService;
    private readonly IRobotService robotService;
    private readonly IPlayerService playerService;
    private readonly IApplicationMessagesService applicationMessagesService;
    public CommandResultEnum CommandResult { get; set; }
    public string? ExecuteResultText { get; set; }

    public CreateRobotCommandService(
        ILogger<CreateRobotCommandService> logger,
        IMapService mapService,
        IRobotService robotService,
        IPlayerService playerService,
        IApplicationMessagesService applicationMessagesService
        )
    {
        this.loggerService = logger;
        this.mapService = mapService;
        this.robotService = robotService;
        this.playerService = playerService;
        this.applicationMessagesService = applicationMessagesService;
    }
    public async Task<bool> Execute()
    {
        try
        {
            Debug.Assert(playerService.ActivePlayer != null);
            if(mapService.ActiveMap == null)
            {
                applicationMessagesService.SetResult(this, CommandResultEnum.ActiveMapNull);
                return false;
            }
            robotService.ActiveRobot = await robotService.CreateRobot(
                playerService.ActivePlayer.PlayerId, mapService.ActiveMap.MapId);
            if (robotService.ActiveRobot == null)
            {
                this.loggerService.LogTrace("CREATEROBOT robot failed");
                applicationMessagesService.SetResult(this, CommandResultEnum.CreateRobotFailed);
                return false;
            }
            this.loggerService.LogTrace("CREATEROBOT command: player {PlayerId}, map {MapId} robot created {RobotId}", 
                playerService.ActivePlayer.PlayerId, mapService.ActiveMap.MapId, robotService.ActiveRobot.RobotId);
            applicationMessagesService.SetResult(this, CommandResultEnum.Ok, CommandResultEnum.RobotCreatedId, robotService.ActiveRobot.RobotId);
            return true;
        }
        catch (Exception ex)
        {
            this.loggerService.LogError(ex, "CREATEROBOT Command unexpected error");
            applicationMessagesService.SetResult(this, CommandResultEnum.UnexpectedError);
            return false;
        }
    }

    public bool TryParse(string[] commandParts)
    {
        if (commandParts.Length != 1)
            return false;
        if (FirstInstruction.Equals(commandParts[0]))
        {
            return true;
        }
        return false;
    }
}

