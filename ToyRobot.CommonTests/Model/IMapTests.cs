using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToyRobot.Common.Model;

namespace ToyRobot.Common.Test.Model;

[TestClass()]
public class IMapTests
{
    internal class LocalMap : IMap
    {
        public int MapId { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
        public LocalMap(int w, int h)
        {
            this.Width = w;
            this.Height = h;
        }
    }
    internal class Point : IMapPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    [TestMethod()]
    [DataRow(0, 0, false)]
    [DataRow(1, 1, true)]
    [DataRow(1, 5, true)]
    [DataRow(5, 1, true)]
    [DataRow(4, 4, true)]
    [DataRow(5, 5, true)]
    [DataRow(6, 6, false)]
    [DataRow(-1, 1, false)]
    [DataRow(1, -1, false)]
    [DataRow(-1, -1, false)]
    [DataRow(5, 6, false)]
    [DataRow(6, 5, false)]
    public void IsInMapTest(int x, int y, bool expected)
    {
        var map = (IMap) new LocalMap(5,5);
        Assert.AreEqual(expected, map.IsInMap(new Point(x, y)));
    }
}
