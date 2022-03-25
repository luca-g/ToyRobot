
namespace ToyRobot.Common.Model;

public interface IRobot
{
    int RobotId { get; }
    IPlayer Player { get; }
    IMap Map { get; }
    IMapPosition? Position { get; }
    Task SetMapPosition(IMapPosition? mapPosition);
}
