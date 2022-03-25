using ToyRobot.Common.Model;

namespace ToyRobot.Common.Services;

public interface ICommandCenterService
{
    string? ExecuteResult { get; }
    bool CommandExecuted { get; }
    IList<ICommand> Commands { get; }
    Task<bool> Execute(string command);
}
