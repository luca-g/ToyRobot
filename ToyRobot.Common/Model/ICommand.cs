namespace ToyRobot.Common.Model;

public interface ICommand
{
    ICommandText CommandInstructions { get; }
    string? ExecuteResultText { get; set; }
    CommandResultEnum CommandResult { get; set; }
    bool TryParse(string[] commandParts);
    Task<bool> Execute(IScenario scenario);
}
