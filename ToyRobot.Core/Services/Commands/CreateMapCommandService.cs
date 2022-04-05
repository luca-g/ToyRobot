using Microsoft.Extensions.Logging;
using System.Diagnostics;
using ToyRobot.Common.Extensions;
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
    private readonly IApplicationMessagesService applicationMessagesService;
    public CommandResultEnum CommandResult { get; set; }
    public string? ExecuteResultText { get; set; }

    private int w;
    private int h;
    public CreateMapCommandService(
        ILogger<CreateMapCommandService> logger, 
        IRobotStepHistoryService robotStepHistoryService, 
        IMapService mapService,
        IApplicationMessagesService applicationMessagesService
        )
    {
        this.loggerService = logger;
        this.robotStepHistoryService = robotStepHistoryService;
        this.mapService = mapService;
        this.applicationMessagesService = applicationMessagesService;
    }
    public async Task<bool> Execute(IScenario scenario)
    {
        var returnValue = false;
        var inException = false;
        try
        {
            Debug.Assert(commandParts != null);
            Debug.Assert(commandParts.Length == 3);
            var map = await mapService.CreateMap(scenario.PlayerId, this.w, this.h);
            if (map == null)
            {
                applicationMessagesService.SetResult(this, CommandResultEnum.MapCreateError);
                this.loggerService.LogError("CREATEMAP command: error creating the map");
                return returnValue;
            }

            await scenario.SetActiveMap(map);

            this.loggerService.LogTrace("CREATEMAP command: map created id {MapId} size {w},{h}", map.MapId, this.w, this.h);
            applicationMessagesService.SetResult(this, CommandResultEnum.Ok, CommandResultEnum.MapCreatedIdWH, map.MapId, this.w, this.h);

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
                await this.robotStepHistoryService.AddStep(commandParts[0], returnValue, this.ExecuteResultText);
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

