using ToyRobot.Common.Model;

namespace ToyRobot.Common.Services;

public interface IRobotStepHistoryService
{
    Task AddResizeMapStepAsync(int width, int height);
    Task AddStep(
        IMapPosition? positionBeforeCommand,
        IMapPosition? positionAfterCommand,
        string command,
        bool commandExecuted,
        string? result
        );
} 
