using ToyRobot.Common.Model;


namespace ToyRobot.API.Model;

public class RobotPosition
{
    public int X { get; set; }
    public int Y { get; set; }
    public MapOrientationEnum Orientation { get; set; }
}
