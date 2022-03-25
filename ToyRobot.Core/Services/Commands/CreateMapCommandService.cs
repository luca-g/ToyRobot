using Microsoft.Extensions.Logging;
using System.Diagnostics;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;

namespace ToyRobot.Core.Services.Commands;

public class CreateMapCommandService : ICommand
{
    public string FirstInstruction => "CREATEMAP";
    public string ConsoleInstruction { get => "CREATEMAP w,h"; }
    private readonly ILogger<CreateMapCommandService> loggerService;
    private string[]? commandParts = null;
    private readonly IRobotStepHistoryService robotStepHistoryService;
    private readonly IMapService mapService;
    private readonly IRobotService robotService;
    public string? ExecuteResult { get; private set; }
    private int w;
    private int h;
    public CreateMapCommandService(
        ILogger<CreateMapCommandService> logger, 
        IRobotStepHistoryService robotStepHistoryService, 
        IMapService mapService,
        IRobotService robotService
        )
    {
        this.loggerService = logger;
        this.robotStepHistoryService = robotStepHistoryService;
        this.mapService = mapService;
        this.robotService = robotService;
    }
    public async Task<bool> Execute()
    {
        var returnValue = false;
        var inException = false;
        try
        {
            Debug.Assert(commandParts != null);
            Debug.Assert(commandParts.Length == 3);
            var map = await mapService.CreateMap(this.w, this.h);
            if (map == null)
            {
                this.loggerService.LogError("CREATEMAP command: error creating the map");
                return returnValue;
            }

            robotService.ActiveRobot = null;

            this.loggerService.LogTrace("CREATEMAP command: map created id {MapId} size {w},{h}", map.MapId, this.w, this.h);
            this.ExecuteResult = $"Map created id {map.MapId}";
            returnValue = true;
            return returnValue;
        }
        catch (Exception ex)
        {
            this.loggerService.LogError(ex, "CREATEMAP Command unexpected error");
            inException = true;
            throw;
        }
        finally
        {
            if(commandParts!=null && !inException)
                await this.robotStepHistoryService.AddStep(commandParts[0], returnValue, this.ExecuteResult);
        }
    }

    public bool TryParse(string[] commandParts)
    {
        if (commandParts.Length != 3)
            return false;
        if (FirstInstruction.Equals(commandParts[0]))
        {
            if (!int.TryParse(commandParts[1], out this.w))
            {
                this.loggerService.LogTrace("CREATEMAP invalid W value: {commandParts[1]}", commandParts[1]);
                return false;
            }
            if (!int.TryParse(commandParts[2], out this.h))
            {
                this.loggerService.LogTrace("CREATEMAP invalid H value: {commandParts[2]}", commandParts[2]);
                return false;
            }
            this.commandParts = commandParts;
            return true;
        }
        return false;
    }
}

