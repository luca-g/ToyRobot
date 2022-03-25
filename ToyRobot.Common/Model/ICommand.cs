namespace ToyRobot.Common.Model;

public interface ICommand
{
    string? ExecuteResult { get => null; }
    string FirstInstruction { get; }
    string ConsoleInstruction { get => FirstInstruction; }
    bool TryParse(string[] commandParts);
    Task<bool> Execute();
}
