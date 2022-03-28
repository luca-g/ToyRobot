using Microsoft.Extensions.Logging;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;

namespace ToyRobot.Core.Services.Commands;

public class SizeCommandService : ICommand
{
    public string FirstInstruction => "SIZE";
    private readonly ILogger<SizeCommandService> loggerService;
    private readonly IMapService mapService;
    private readonly IApplicationMessagesService applicationMessagesService;
    public string? ExecuteResultText { get; set; }
    public CommandResultEnum CommandResult { get; set; }

    public SizeCommandService(
        ILogger<SizeCommandService> logger,
        IMapService mapService,
        IApplicationMessagesService applicationMessagesService
        )
    {
        this.loggerService = logger;
        this.mapService = mapService;
        this.applicationMessagesService = applicationMessagesService;
    }
    public Task<bool> Execute()
    {
        try
        {
            if (mapService.ActiveMap == null)
            {
                this.applicationMessagesService.SetResult(this, CommandResultEnum.ActiveMapNull);
                this.loggerService.LogTrace("SIZE command result: {result}", this.ExecuteResultText);
            }
            else
            {
                this.ExecuteResultText = mapService.ActiveMap?.Size();
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
        return FirstInstruction.Equals(commandParts[0]);
    }
}

