using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using ToyRobot.MockHelper;

namespace ToyRobot.Core.Services.Commands.Tests;

[TestClass()]
public class PlaceCommandServiceTests
{
    [TestMethod()]
    [DataRow("PLACE,5,5,NORTH", true)]
    [DataRow("PLACE,5,5,NORT", false)]
    [DataRow("PLACE,D,5,NORTH", false)]
    [DataRow("PLAC,5,5,NORTH", false)]
    public void TryParseTest(string command, bool parsed)
    {
        var mock = new MockServicesHelper<PlaceCommandService>();
        var mapCommandService = new PlaceCommandService(
            mock.Logger.Object,
            mock.RobotService.Object);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(parsed, mapCommandService.TryParse(parts));
    }

    [TestMethod()]
    [DataRow("PLACE,5,5,NORTH")]
    public async Task ExecuteTest(string command)
    {
        var mock = new MockServicesHelper<PlaceCommandService>();
        mock.ActivePlayerSetupProperty(1)
            .ActiveMapSetupProperty(1)
            .ActiveRobotSetupProperty(1);
        var mapCommandService = new PlaceCommandService(
            mock.Logger.Object,
            mock.RobotService.Object);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, mapCommandService.TryParse(parts));

        var result = await mapCommandService.Execute();

        Assert.AreEqual(true, result);
        Assert.IsTrue(mapCommandService.ExecuteResult?.StartsWith("OK"));
        Assert.AreEqual(1, mock.SetPositionCalled);

    }
    [TestMethod()]
    [DataRow("PLACE,5,5,NORTH")]
    public async Task ExecuteTest_NoActiveRobot(string command)
    {
        var mock = new MockServicesHelper<PlaceCommandService>();
        mock.ActivePlayerSetupProperty(1)
            .ActiveMapSetupProperty(1);
        var mapCommandService = new PlaceCommandService(
            mock.Logger.Object,
            mock.RobotService.Object);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, mapCommandService.TryParse(parts));

        var result = await mapCommandService.Execute();

        Assert.AreEqual(false, result);
        Assert.IsTrue(mapCommandService.ExecuteResult?.StartsWith("The current map has no robots"));
        Assert.AreEqual(0, mock.SetPositionCalled);
    }
    [TestMethod()]
    [DataRow("PLACE,5,5,NORTH")]
    public async Task ExecuteTest_RobotOutOfMap(string command)
    {
        var mock = new MockServicesHelper<PlaceCommandService>();
        mock.ActivePlayerSetupProperty(1)
            .ActiveMapSetupProperty(1, false)
            .ActiveRobotSetupProperty(1);
        var mapCommandService = new PlaceCommandService(
            mock.Logger.Object,
            mock.RobotService.Object);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, mapCommandService.TryParse(parts));

        var result = await mapCommandService.Execute();

        Assert.AreEqual(false, result);
        Assert.IsTrue(mapCommandService.ExecuteResult?.StartsWith("The object is outside the map"));
        Assert.AreEqual(0, mock.SetPositionCalled);
    }
}
