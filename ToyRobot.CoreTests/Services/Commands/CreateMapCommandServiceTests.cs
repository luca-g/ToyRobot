using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using ToyRobot.MockHelper;

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
        var mock = new MockServicesHelper<CreateMapCommandService>();
        var mapCommandService = new CreateMapCommandService(
            mock.Logger.Object,
            mock.RobotStepHistoryService.Object,
            mock.MapService.Object,
            mock.RobotService.Object);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(parsed, mapCommandService.TryParse(parts));
    }

    [TestMethod()]
    [DataRow("CREATEMAP,5,5")]
    public async Task ExecuteTest(string command)
    {
        var mock = new MockServicesHelper<CreateMapCommandService>();
        mock.CreateMapSetup();
        var mapCommandService = new CreateMapCommandService(
            mock.Logger.Object,
            mock.RobotStepHistoryService.Object,
            mock.MapService.Object,
            mock.RobotService.Object);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, mapCommandService.TryParse(parts));

         var result = await mapCommandService.Execute();

        Assert.AreEqual(true, result);
        Assert.IsTrue(mapCommandService.ExecuteResult?.StartsWith("Map created id"));
    }
    [TestMethod()]
    [DataRow("CREATEMAP,5,5")]
    public async Task ExecuteTest_NoMapCreated(string command)
    {
        var mock = new MockServicesHelper<CreateMapCommandService>();
        var mapCommandService = new CreateMapCommandService(
            mock.Logger.Object,
            mock.RobotStepHistoryService.Object,
            mock.MapService.Object,
            mock.RobotService.Object);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, mapCommandService.TryParse(parts));

        var result = await mapCommandService.Execute();

        Assert.AreEqual(false, result);
    }
}
