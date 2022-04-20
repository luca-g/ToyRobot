using ToyRobot.Common.Model;
using ToyRobot.Common.Services;
using ToyRobot.Core.Model;

namespace ToyRobot.Core.Services;
public class CoreFactoryService : ICoreFactoryService
{
    public ICommandTextBuilder CreateCommandInstructionsBuilder()
    {
        return new CommandTextBuilder();
    }
}
