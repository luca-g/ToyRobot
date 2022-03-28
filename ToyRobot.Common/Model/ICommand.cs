namespace ToyRobot.Common.Model;

public interface ICommand
{
    string? ExecuteResultText { get; set; }
    CommandResultEnum CommandResult { get; set; }
    string FirstInstruction { get; }
    string ConsoleInstruction { get => FirstInstruction; }
    bool TryParse(string[] commandParts);
    Task<bool> Execute();
}
