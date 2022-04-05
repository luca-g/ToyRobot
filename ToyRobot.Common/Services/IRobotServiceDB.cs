using ToyRobot.Common.Model;

namespace ToyRobot.Common.Services;

public interface IRobotServiceDB
{
    Task SaveMapPosition(IRobot robot, IMapPosition? mapPosition, int playerId);
}
