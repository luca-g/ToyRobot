using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToyRobot.Model;
using ToyRobot.Configuration;
using ToyRobot.Services;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ToyRobot.Tests;

[TestClass()]
public class MapServiceTests
{
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
    public void IsInMapTest()
    {
        var mockLogger = new Mock<ILogger<MapService>>();

        var mapSettings = new MapSettings { Height = 5, Width = 5 };
        var mockMapSettings = new Mock<IOptions<MapSettings>>();
        mockMapSettings.Setup(t=>t.Value).Returns(mapSettings);

        var map = new MapService(mockLogger.Object, mockMapSettings.Object);
        Assert.IsTrue(map.IsInMap(new Point(0, 0)));
        Assert.IsTrue(map.IsInMap(new Point(4, 4)));
        Assert.IsTrue(!map.IsInMap(new Point(5, 5)));
        Assert.IsTrue(!map.IsInMap(new Point(-1, -1)));
        Assert.IsTrue(map.IsInMap(new Point(0, 4)));
        Assert.IsTrue(!map.IsInMap(new Point(0, 5)));
        Assert.IsTrue(!map.IsInMap(new Point(0, -1)));
        Assert.IsTrue(map.IsInMap(new Point(4, 0)));
        Assert.IsTrue(!map.IsInMap(new Point(5, 0)));
        Assert.IsTrue(!map.IsInMap(new Point(-1, 0)));

    }
}
