using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Extensions.Logging;
using ToyRobot.Configuration;
using Microsoft.Extensions.Options;

namespace ToyRobot.Services.Tests;

[TestClass()]
public class RobotServiceTests
{
    private RobotService _robotService;
    public RobotServiceTests()
    {
        var mockLogger = new Mock<ILogger<MapService>>();

        var mapSettings = new MapSettings { Height = 5, Width = 5 };
        var mockMapSettings = new Mock<IOptions<MapSettings>>();
        mockMapSettings.Setup(t => t.Value).Returns(mapSettings);

        var mapService = new MapService(mockLogger.Object, mockMapSettings.Object);

        var mockRobotLogger = new Mock<ILogger<RobotService>>();

        _robotService = new RobotService(mockRobotLogger.Object, mapService);
    }

    [TestMethod()]
    public void ExecuteTest()
    {
        {
            string? result;
            Assert.IsTrue(_robotService.Execute("PLACE 0,0,NORTH", out result));
            Assert.IsTrue(_robotService.Execute("MOVE", out result));
            Assert.IsTrue(_robotService.Execute("REPORT", out result));
            Assert.AreEqual(result, "0,1,NORTH");
        }
        {
            string? result;
            Assert.IsTrue(_robotService.Execute("PLACE 0,0,NORTH", out result));
            Assert.IsTrue(_robotService.Execute("LEFT", out result));
            Assert.IsTrue(_robotService.Execute("REPORT", out result));
            Assert.AreEqual(result, "0,0,WEST");
        }
       {
            string? result;
            Assert.IsTrue(_robotService.Execute("PLACE 1,2,EAST", out result));
            Assert.IsTrue(_robotService.Execute("MOVE", out result));
            Assert.IsTrue(_robotService.Execute("MOVE", out result));
            Assert.IsTrue(_robotService.Execute("LEFT", out result));
            Assert.IsTrue(_robotService.Execute("MOVE", out result));
            Assert.IsTrue(_robotService.Execute("REPORT", out result));
            Assert.AreEqual(result, "3,3,NORTH");
        }
    }
}
