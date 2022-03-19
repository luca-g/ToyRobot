using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Extensions.Logging;
using ToyRobot.Configuration;
using Microsoft.Extensions.Options;
using ToyRobot.Common.Services;

namespace ToyRobot.Services.Tests;

[TestClass()]
public class RobotServiceTests
{
    private readonly RobotService _robotService;
    public RobotServiceTests()
    {
        var mockLogger = new Mock<ILogger<MapService>>();

        var mapSettings = new MapSettings { Height = 5, Width = 5 };
        var mockMapSettings = new Mock<IOptions<MapSettings>>();
        mockMapSettings.Setup(t => t.Value).Returns(mapSettings);

        var mapService = new MapService(mockLogger.Object, mockMapSettings.Object);

        var mockRobotLogger = new Mock<ILogger<RobotService>>();

        var mockRobotStepHistoryService = new Mock<IRobotStepHistoryService>();

        _robotService = new RobotService(mockRobotLogger.Object, mapService, mockRobotStepHistoryService.Object);
    }

    [TestMethod()]
    public async void ExecuteTest()
    {
        {
            Assert.IsTrue(await _robotService.Execute("PLACE 0,0,NORTH"));
            Assert.IsTrue(await _robotService.Execute("MOVE"));
            Assert.IsTrue(await _robotService.Execute("REPORT"));
            Assert.AreEqual(_robotService.ExecuteResult, "0,1,NORTH");
        }
        {
            Assert.IsTrue(await _robotService.Execute("PLACE 0,0,NORTH"));
            Assert.IsTrue(await _robotService.Execute("LEFT"));
            Assert.IsTrue(await _robotService.Execute("REPORT"));
            Assert.AreEqual(_robotService.ExecuteResult, "0,0,WEST");
        }
       {
            Assert.IsTrue(await _robotService.Execute("PLACE 1,2,EAST"));
            Assert.IsTrue(await _robotService.Execute("MOVE"));
            Assert.IsTrue(await _robotService.Execute("MOVE"));
            Assert.IsTrue(await _robotService.Execute("LEFT"));
            Assert.IsTrue(await _robotService.Execute("MOVE"));
            Assert.IsTrue(await _robotService.Execute("REPORT"));
            Assert.AreEqual(_robotService.ExecuteResult, "3,3,NORTH");
        }
    }
}
