namespace ToyRobot.Model;

public class MapPosition : IMapPoint
{
    public MapOrientationEnum Orientation;
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

    public void Left()
    {
        switch (this.Orientation)
        {
            case MapOrientationEnum.NORTH:
                this.Orientation = MapOrientationEnum.WEST;
                break;
            case MapOrientationEnum.EAST:
                this.Orientation = MapOrientationEnum.NORTH;
                break;
            case MapOrientationEnum.SOUTH:
                this.Orientation = MapOrientationEnum.EAST;
                break;
            case MapOrientationEnum.WEST:
                this.Orientation = MapOrientationEnum.SOUTH;
                break;
            default:
                throw new InvalidOperationException();
        }
    }

    public void Right()
    {
        switch (this.Orientation)
        {
            case MapOrientationEnum.NORTH:
                this.Orientation = MapOrientationEnum.EAST;
                break;
            case MapOrientationEnum.EAST:
                this.Orientation = MapOrientationEnum.SOUTH;
                break;
            case MapOrientationEnum.SOUTH:
                this.Orientation = MapOrientationEnum.WEST;
                break;
            case MapOrientationEnum.WEST:
                this.Orientation = MapOrientationEnum.NORTH;
                break;
            default:
                throw new InvalidOperationException();
        }
    }

    public MapPosition Move()
    {
        switch (this.Orientation)
        {
            case MapOrientationEnum.NORTH:
                return new MapPosition(this.X, this.Y + 1, this.Orientation);
            case MapOrientationEnum.EAST:
                return new MapPosition(this.X + 1, this.Y, this.Orientation);
            case MapOrientationEnum.SOUTH:
                return new MapPosition(this.X, this.Y - 1, this.Orientation);
            case MapOrientationEnum.WEST:
                return new MapPosition(this.X - 1, this.Y, this.Orientation);
            default:
                throw new InvalidOperationException();
        }
    }
}
