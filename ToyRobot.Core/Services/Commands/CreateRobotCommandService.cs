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
    public string? ExecuteResult { get; private set; }
    public CreateRobotCommandService(
        ILogger<CreateRobotCommandService> logger,
        IMapService mapService,
        IRobotService robotService,
        IPlayerService playerService
        )
    {
        this.loggerService = logger;
        this.mapService = mapService;
        this.robotService = robotService;
        this.playerService = playerService;
    }
    public async Task<bool> Execute()
    {
        var returnValue = false;
        try
        {
            Debug.Assert(playerService.ActivePlayer != null);
            if(mapService.ActiveMap == null)
            {
                this.ExecuteResult = "The map is not selected";
                return false;
            }
            robotService.ActiveRobot = await robotService.CreateRobot(
                playerService.ActivePlayer.PlayerId, mapService.ActiveMap.MapId);
            if (robotService.ActiveRobot == null)
            {
                this.loggerService.LogTrace("CREATEROBOT robot failed");
                this.ExecuteResult = "Create robot failed";
                return returnValue;
            }
            this.loggerService.LogTrace("CREATEROBOT command: player {PlayerId}, map {MapId} robot created {RobotId}", 
                playerService.ActivePlayer.PlayerId, mapService.ActiveMap.MapId, robotService.ActiveRobot.RobotId);
            this.ExecuteResult = $"Robot created id {robotService.ActiveRobot.RobotId}";
            returnValue = true;
            return returnValue;
        }
        catch (Exception ex)
        {
            this.loggerService.LogError(ex, "CREATEROBOT Command unexpected error");
            throw;
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

