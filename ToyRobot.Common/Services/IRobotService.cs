using ToyRobot.Common.Model;

namespace ToyRobot.Common.Services;

public interface IRobotService
{
    IRobot? ActiveRobot { get; set; }
    Task<IRobot> CreateRobot(int playerId, int mapId);
    Task<IList<IRobot>> LoadRobots(int playerId, int? mapId);
    Task SetMapPosition(IRobot robot, IMapPosition? mapPosition);
}
