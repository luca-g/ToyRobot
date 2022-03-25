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
    private readonly IRobotService robotService;
    public CommandCenterService(
        ILogger<CommandCenterService> logger, 
        IServiceProvider serviceProvider,
        IRobotStepHistoryService robotStepHistoryService,
        IRobotService robotService
        )
    {
        this.Commands = serviceProvider.GetServices<ICommand>().OrderBy(t=>t.ConsoleInstruction).ToList();
        this.loggerService = logger;
        this.robotStepHistoryService = robotStepHistoryService;
        this.robotService = robotService;
    }
    public async Task<bool> Execute(string command)
    {
        var robot = robotService.ActiveRobot;
        this.ExecuteResult = null;
        this.CommandExecuted = false;
        var previousMapPosition = robot?.Position;
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
                    executed |= await commandObj.Execute();
                    if(commandObj.ExecuteResult != null)
                        results.AppendLine(commandObj.ExecuteResult);
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
            await this.robotStepHistoryService.AddStep(previousMapPosition, robot?.Position, command, this.CommandExecuted, this.ExecuteResult);

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
