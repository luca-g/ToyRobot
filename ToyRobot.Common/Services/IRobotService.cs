using ToyRobot.Common.Model;

namespace ToyRobot.Common.Services;

public interface IRobotService
{
    Task<IRobot> CreateRobot(int playerId, int mapId);
    Task<IList<IRobot>> LoadRobots(int playerId, int? mapId);
}
