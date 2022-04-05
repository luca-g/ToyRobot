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
    private readonly IRobotService robotService;
    private readonly IApplicationMessagesService applicationMessagesService;
    public CommandResultEnum CommandResult { get; set; }
    public string? ExecuteResultText { get; set; }

    public CreateRobotCommandService(
        ILogger<CreateRobotCommandService> logger,
        IRobotService robotService,
        IApplicationMessagesService applicationMessagesService
        )
    {
        this.loggerService = logger;
        this.robotService = robotService;
        this.applicationMessagesService = applicationMessagesService;
    }
    public async Task<bool> Execute(IScenario scenario)
    {
        try
        {
            if(!scenario.IsMapSet ||  scenario.MapId == null)
            {
                applicationMessagesService.SetResult(this, CommandResultEnum.ActiveMapNull);
                return false;
            }
            var robot = await robotService.CreateRobot(scenario.PlayerId, scenario.MapId.Value);
            if (robot == null)
            {
                this.loggerService.LogTrace("CREATEROBOT robot failed");
                applicationMessagesService.SetResult(this, CommandResultEnum.CreateRobotFailed);
                return false;
            }
            await scenario.SetActiveRobot(robot);
            this.loggerService.LogTrace("CREATEROBOT command: player {PlayerId}, map {MapId} robot created {RobotId}", 
                scenario.PlayerId, scenario.MapId, scenario.RobotId);
            applicationMessagesService.SetResult(this, CommandResultEnum.Ok, CommandResultEnum.RobotCreatedId, scenario.RobotId);
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

