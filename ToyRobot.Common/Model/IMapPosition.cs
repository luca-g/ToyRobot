namespace ToyRobot.Common.Model;

public interface IMapPosition : IMapPoint
{
    MapOrientationEnum Orientation { get; }
    IMapPosition Left();
    IMapPosition Right();
    IMapPosition Move();
    string ToString() { return $"{X},{Y},{Orientation}"; }
}
