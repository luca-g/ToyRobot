namespace ToyRobot.Common.Model;

public interface IMap
{
    int MapId { get; }
    int Width { get; }
    int Height { get; }
    string Size()
    {
        return $"Width {Width}, Height {Height}";
    }
    bool IsInMap(IMapPoint point)
    {
        return point.X < Width && point.Y < Height && point.X>=0 && point.Y>=0;
    }
    static int Compare(IMap a, IMap b)
    {
        if (a.Width.Equals(b.Width))
            return a.Height.CompareTo(b.Height);
        return a.Width.CompareTo(b.Width);
    }
}
