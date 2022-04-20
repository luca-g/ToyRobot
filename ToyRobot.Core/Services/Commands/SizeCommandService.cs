using Microsoft.Extensions.Logging;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;

namespace ToyRobot.Core.Services.Commands;

public class SizeCommandService : ICommand
{
    public ICommandText CommandInstructions { get; private set; }
    private readonly ILogger<SizeCommandService> loggerService;
    private readonly IApplicationMessagesService applicationMessagesService;
    public string? ExecuteResultText { get; set; }
    public CommandResultEnum CommandResult { get; set; }

    public SizeCommandService(
        ICoreFactoryService coreFactoryService,
        ILogger<SizeCommandService> logger,
        IApplicationMessagesService applicationMessagesService
        )
    {
        this.loggerService = logger;
        this.applicationMessagesService = applicationMessagesService;
        this.CommandInstructions =
            coreFactoryService.CreateCommandInstructionsBuilder()
            .SetFirstInstruction("SIZE")
            .Build();
    }
    public Task<bool> Execute(IScenario scenario)
    {
        try
        {
            if (!scenario.IsMapSet)
            {
                this.applicationMessagesService.SetResult(scenario.Language, this, CommandResultEnum.ActiveMapNull);
                this.loggerService.LogTrace("SIZE command result: {result}", this.ExecuteResultText);
            }
            else
            {
                this.ExecuteResultText = scenario.MapSize();
                this.CommandResult = CommandResultEnum.Ok;
                this.loggerService.LogTrace("SIZE command result: {result}", this.ExecuteResultText);
            }
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            this.loggerService.LogError(ex, "SIZE Command unexpected error");
            return Task.FromException<bool>(ex);
        }
    }

    public bool TryParse(string[] commandParts)
    {
        if (commandParts.Length != 1)
            return false;
        return CommandInstructions.CommandName.Equals(commandParts[0]);
    }
}

