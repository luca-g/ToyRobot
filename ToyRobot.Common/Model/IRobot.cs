
namespace ToyRobot.Common.Model;

public interface IRobot
{
    int RobotId { get; }
    IMapPosition? Position { get; set; }
}
