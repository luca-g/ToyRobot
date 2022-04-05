using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;

namespace ToyRobot.Core.Services;

public class CommandCenterService : ICommandCenterService
{
    public string? ExecuteResult { get; private set; }
    public bool CommandExecuted { get; private set; }
    public IList<ICommand> Commands { get; }
    private readonly ILogger<CommandCenterService> loggerService;
    private readonly IRobotStepHistoryService robotStepHistoryService;
    public CommandCenterService(
        ILogger<CommandCenterService> logger, 
        IServiceProvider serviceProvider,
        IRobotStepHistoryService robotStepHistoryService
    )
    {
        this.Commands = serviceProvider.GetServices<ICommand>().OrderBy(t=>t.ConsoleInstruction).ToList();
        this.loggerService = logger;
        this.robotStepHistoryService = robotStepHistoryService;
    }
    public async Task<bool> Execute(IScenario scenario, string command)
    {
        this.ExecuteResult = null;
        this.CommandExecuted = false;
        var previousMapPosition = scenario.RobotPosition;
        try
        {
            this.loggerService.LogTrace("Execute command: {command}", command);
            if (string.IsNullOrEmpty(command))
            {
                this.loggerService.LogTrace("Command is null or empty");
                this.ExecuteResult = "Invalid command";
                return false;
            }
            var commandParts = command.Split(new char[] { ' ', ',' });
            if (commandParts.Length > 0 && commandParts[0].Length>0)
            {
                var commands = this.Commands.Where(t => t.TryParse(commandParts)).ToList();
                var executed = false;
                var results = new StringBuilder();
                foreach (var commandObj in commands)
                {
                    executed |= await commandObj.Execute(scenario);
                    if(commandObj.ExecuteResultText != null)
                        results.AppendLine(commandObj.ExecuteResultText);
                }
                this.ExecuteResult = results.Length > 0 ? results.ToString() : null;
                if(this.ExecuteResult != null && this.ExecuteResult.EndsWith("\r\n"))
                {
#pragma warning disable IDE0057 // Use range operator
                    this.ExecuteResult = this.ExecuteResult.Substring(0, this.ExecuteResult.Length - "\r\n".Length);
#pragma warning restore IDE0057 // Use range operator
                }
                this.CommandExecuted = executed;
            }
            await this.robotStepHistoryService.AddStep(previousMapPosition, scenario.RobotPosition, command, this.CommandExecuted, this.ExecuteResult);

            if (this.ExecuteResult == null && !this.CommandExecuted)
            {
                this.ExecuteResult = "Invalid command";
            }
            else if (this.ExecuteResult == null && this.CommandExecuted)
            {
                this.ExecuteResult = "OK";
            }
            return true;
        }
        catch (Exception ex)
        {
            this.ExecuteResult = "Unexpected error";
            loggerService.LogError(ex, "Execute error");
            return false;
        }
    }
}
