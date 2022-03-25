using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Moq;
using Microsoft.Extensions.Logging;
using ToyRobot.Common.Services;
using ToyRobot.Common.Model;

namespace ToyRobot.Core.Services.Commands.Tests;

[TestClass()]
public class CreateMapCommandServiceTests
{
    [TestMethod()]
    [DataRow("CREATEMAP,5,5", true)]
    [DataRow("CREATEMAP 5,5", true)]
    [DataRow("CREATEMAP 5, 5", false)]
    [DataRow("CREATEMA 5,5", false)]
    [DataRow("CREATEMAP5,5", false)]
    [DataRow("CREATEMAP x,5", false)]
    [DataRow("CREATEMAP 5,5,", false)]
    [DataRow("CREATEMAP 5,5,5", false)]
    public void TryParseTest(string command, bool parsed)
    {
        var mockLogger = new Mock<ILogger<CreateMapCommandService>>();
        var mockMapService = new Mock<IMapService>();
        var mockRobotService = new Mock<IRobotService>();
        var mockRobotStepHistoryService = new Mock<IRobotStepHistoryService>();

        var mapCommandService = new CreateMapCommandService(
            mockLogger.Object,
            mockRobotStepHistoryService.Object,
            mockMapService.Object,
            mockRobotService.Object);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(parsed, mapCommandService.TryParse(parts));
    }

    [TestMethod()]
    [DataRow("CREATEMAP,5,5")]
    public async Task ExecuteTest(string command)
    {
        var mockLogger = new Mock<ILogger<CreateMapCommandService>>();
        var mockMapService = new Mock<IMapService>();
        mockMapService
            .Setup(t=>t.CreateMap(It.IsAny<int>(),It.IsAny<int>()))
            .Returns(Task.FromResult(new Mock<IMap>().Object));

        var mockRobotService = new Mock<IRobotService>();
        var mockRobotStepHistoryService = new Mock<IRobotStepHistoryService>();

        var mapCommandService = new CreateMapCommandService(
            mockLogger.Object,
            mockRobotStepHistoryService.Object,
            mockMapService.Object,
            mockRobotService.Object);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, mapCommandService.TryParse(parts));

        var mockRobot = new Mock<IRobot>();
        mockRobotService.Object.ActiveRobot = mockRobot.Object;
        var result = await mapCommandService.Execute();
        Assert.AreEqual(true, result);
        Assert.IsTrue(mapCommandService.ExecuteResult?.StartsWith("Map created id"));
    }
}
