using ToyRobot.Common.Model;

namespace ToyRobot.Core.Model;

public class MapPosition : IMapPosition, IMapPoint
{
    public MapOrientationEnum Orientation { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }

    public MapPosition(int x, int y, MapOrientationEnum orientation)
    {
        this.X = x;
        this.Y = y;
        this.Orientation = orientation;
    }

    new public string ToString()
    {
        return string.Format("{0},{1},{2}", this.X, this.Y, this.Orientation);
    }

    public IMapPosition Left()
    {
        var orientation = this.Orientation switch
        {
            MapOrientationEnum.NORTH => MapOrientationEnum.WEST,
            MapOrientationEnum.EAST => MapOrientationEnum.NORTH,
            MapOrientationEnum.SOUTH => MapOrientationEnum.EAST,
            MapOrientationEnum.WEST => MapOrientationEnum.SOUTH,
            _ => throw new InvalidOperationException(),
        };
        return new MapPosition(this.X, this.Y, orientation);
    }

    public IMapPosition Right()
    {
        var orientation = this.Orientation switch
        {
            MapOrientationEnum.NORTH => MapOrientationEnum.EAST,
            MapOrientationEnum.EAST => MapOrientationEnum.SOUTH,
            MapOrientationEnum.SOUTH => MapOrientationEnum.WEST,
            MapOrientationEnum.WEST => MapOrientationEnum.NORTH,
            _ => throw new InvalidOperationException(),
        };
        return new MapPosition(this.X, this.Y, orientation);
    }

    public IMapPosition Move()
    {
        return this.Orientation switch
        {
            MapOrientationEnum.NORTH => new MapPosition(this.X, this.Y + 1, this.Orientation),
            MapOrientationEnum.EAST => new MapPosition(this.X + 1, this.Y, this.Orientation),
            MapOrientationEnum.SOUTH => new MapPosition(this.X, this.Y - 1, this.Orientation),
            MapOrientationEnum.WEST => new MapPosition(this.X - 1, this.Y, this.Orientation),
            _ => throw new InvalidOperationException(),
        };
    }
}
