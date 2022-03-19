using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToyRobot.Model;
using ToyRobot.Configuration;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ToyRobot.Common.Model;

namespace ToyRobot.Services.Tests;

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

    readonly MapService _mapService;

    public MapServiceTests()
    {
        var mockLogger = new Mock<ILogger<MapService>>();

        var mapSettings = new MapSettings { Height = 5, Width = 5 };
        var mockMapSettings = new Mock<IOptions<MapSettings>>();
        mockMapSettings.Setup(t => t.Value).Returns(mapSettings);

        _mapService = new MapService(mockLogger.Object, mockMapSettings.Object);
    }

    [TestMethod()]
    public void IsInMapTest()
    {
        _mapService.SetMapSize(5, 5);
        Assert.IsTrue(_mapService.IsInMap(new Point(0, 0)));
        Assert.IsTrue(_mapService.IsInMap(new Point(4, 4)));
        Assert.IsTrue(!_mapService.IsInMap(new Point(5, 5)));
        Assert.IsTrue(!_mapService.IsInMap(new Point(-1, -1)));
        Assert.IsTrue(_mapService.IsInMap(new Point(0, 4)));
        Assert.IsTrue(!_mapService.IsInMap(new Point(0, 5)));
        Assert.IsTrue(!_mapService.IsInMap(new Point(0, -1)));
        Assert.IsTrue(_mapService.IsInMap(new Point(4, 0)));
        Assert.IsTrue(!_mapService.IsInMap(new Point(5, 0)));
        Assert.IsTrue(!_mapService.IsInMap(new Point(-1, 0)));
    }
    [TestMethod()]
    public void SetMapSizeTest()
    {
        _mapService.SetMapSize(5, 5);
        Assert.IsTrue(_mapService.Width == 5);
        Assert.IsTrue(_mapService.Height == 5);
        _mapService.SetMapSize(15, 15);
        Assert.IsTrue(_mapService.Width == 15);
        Assert.IsTrue(_mapService.Height == 15);
    }
}
