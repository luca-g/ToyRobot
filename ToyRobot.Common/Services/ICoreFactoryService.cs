using ToyRobot.Common.Model;

namespace ToyRobot.Common.Services;

public interface ICoreFactoryService
{
    ICommandTextBuilder CreateCommandInstructionsBuilder();
}
