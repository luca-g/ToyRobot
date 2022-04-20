using Microsoft.Extensions.Logging;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;

namespace ToyRobot.Core.Services.Commands;

public class RightCommandService : ICommand
{
    public ICommandText CommandInstructions { get; private set; }
    public string? ExecuteResultText { get; set; }
    public CommandResultEnum CommandResult { get; set; }

    private readonly ILogger<RightCommandService> loggerService;
    private readonly IApplicationMessagesService applicationMessagesService;
    public RightCommandService(
        ICoreFactoryService coreFactoryService,
        ILogger<RightCommandService> logger, 
        IApplicationMessagesService applicationMessagesService
        )
    {
        this.loggerService = logger;
        this.applicationMessagesService = applicationMessagesService;
        this.CommandInstructions =
            coreFactoryService.CreateCommandInstructionsBuilder()
            .SetFirstInstruction("RIGHT")
            .Build();
    }
    public async Task<bool> Execute(IScenario scenario)
    {
        try
        {
            if (!scenario.IsRobotSet)
            {
                this.loggerService.LogTrace("RIGHT command: active robot is null");
                applicationMessagesService.SetResult(scenario.Language, this, CommandResultEnum.ActiveRobotNull);
                return false;
            }
            if (!scenario.IsRobotDeployed || scenario.RobotPosition==null)
            {
                this.loggerService.LogTrace("RIGHT command: The robot is not in the map");
                applicationMessagesService.SetResult(scenario.Language, this, CommandResultEnum.RobotPositionNull);
                return false;
            }
            var newPosition = scenario.RobotPosition.Right();
            await scenario.SetMapPosition(newPosition);
            this.loggerService.LogTrace("RIGHT command: robot moved to position {Orientation}", newPosition.Orientation);
            applicationMessagesService.SetResult(scenario.Language, this, CommandResultEnum.Ok);
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
        return CommandInstructions.CommandName.Equals(commandParts[0]);
    }
}

