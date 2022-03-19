using ToyRobot.Common.Model;

namespace ToyRobot.Common.Services;

public class DummyRobotStepHistoryService : IRobotStepHistoryService
{
    public Task AddResizeMapStepAsync(int width, int height)
    {
        return Task.CompletedTask;
    }

    public Task AddStep(IMapPosition? positionBeforeCommand, IMapPosition? positionAfterCommand, string command, bool commandExecuted, string? result)
    {
        return Task.CompletedTask;
    }
}
