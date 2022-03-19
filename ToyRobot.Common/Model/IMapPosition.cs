namespace ToyRobot.Common.Model;

public interface IMapPosition 
{
    public MapOrientationEnum Orientation { get; }
    public int X { get; }
    public int Y { get; }
}
