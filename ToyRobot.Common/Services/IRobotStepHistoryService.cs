using ToyRobot.Common.Model;

namespace ToyRobot.Common.Services;

public interface IRobotStepHistoryService
{
    Task AddStep(
        IMapPosition? positionBeforeCommand,
        IMapPosition? positionAfterCommand,
        string command,
        bool commandExecuted,
        string? result
        )
    { return Task.CompletedTask; }
    Task AddStep(
        string command,
        bool commandExecuted,
        string? result
        )
    { return this.AddStep(null, null, command, commandExecuted, result); }
}
