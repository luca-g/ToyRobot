using Microsoft.Extensions.Logging;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;

namespace ToyRobot.Core.Services.Commands;

public class ReportCommandService : ICommand
{
    public string FirstInstruction => "REPORT";
    private readonly ILogger<ReportCommandService> loggerService;
    private readonly IRobotService robotService;
    public string? ExecuteResult { get; private set; }
    public ReportCommandService(
        ILogger<ReportCommandService> logger,
        IRobotService robotService)
    {
        this.loggerService = logger;
        this.robotService = robotService;
    }
    public Task<bool> Execute()
    {
        var robot = robotService.ActiveRobot;
        try
        {
            if (robot == null)
            {
                this.loggerService.LogTrace("REPORT command: active robot is null");
                this.ExecuteResult = "Robot out of map";
                return Task.FromResult(false);
            }
            if (robot.Position == null)
            {
                this.ExecuteResult = "Robot out of map";
                this.loggerService.LogTrace("REPORT command result: {ExecuteResult}", this.ExecuteResult);
            }
            else
            {
                this.ExecuteResult = robot.Position.ToString();
                this.loggerService.LogTrace("REPORT command result: {ExecuteResult}", this.ExecuteResult);
            }
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            this.loggerService.LogError(ex, "REPORT Command unexpected error");
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

