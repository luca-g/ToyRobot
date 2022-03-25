using Microsoft.Extensions.Logging;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;

namespace ToyRobot.Core.Services.Commands;

public class SizeCommandService : ICommand
{
    public string FirstInstruction => "SIZE";
    private readonly ILogger<SizeCommandService> loggerService;
    private readonly IMapService mapService;
    public string? ExecuteResult { get; private set; }
    public SizeCommandService(
        ILogger<SizeCommandService> logger,
        IMapService mapService)
    {
        this.loggerService = logger;
        this.mapService = mapService;
    }
    public Task<bool> Execute()
    {
        try
        {
            if (mapService.ActiveMap == null)
            {
                this.ExecuteResult = "Map not set";
                this.loggerService.LogTrace("SIZE command result: {result}", this.ExecuteResult);
            }
            else
            {
                this.ExecuteResult = mapService.ActiveMap?.Size();
                this.loggerService.LogTrace("SIZE command result: {result}", this.ExecuteResult);
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

