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
    private readonly IApplicationMessagesService applicationMessagesService;
    public LeftCommandService(
        ILogger<LeftCommandService> logger,
        IApplicationMessagesService applicationMessagesService
        )
    {
        this.loggerService = logger;
        this.applicationMessagesService = applicationMessagesService;
    }
    public async Task<bool> Execute(IScenario scenario)
    {
        try
        {
            if (!scenario.IsRobotSet)
            {
                applicationMessagesService.SetResult(scenario.Language, this, CommandResultEnum.ActiveRobotNull);
                this.loggerService.LogTrace("LEFT command: active robot is null");
                return false;
            }
            if (!scenario.IsRobotDeployed)
            {
                applicationMessagesService.SetResult(scenario.Language, this, CommandResultEnum.RobotPositionNull);
                this.loggerService.LogTrace("LEFT command: The robot is not in the map");
                return false;
            }
            var newPosition = scenario.RobotPosition?.Left();
            if(newPosition == null)
            {
                throw new Exception("Unexpected null robot position");
            }
            await scenario.SetMapPosition(newPosition);
            this.loggerService.LogTrace("LEFT command: robot moved to position {Orientation}", newPosition.Orientation);
            applicationMessagesService.SetResult(scenario.Language, this, CommandResultEnum.Ok);
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

