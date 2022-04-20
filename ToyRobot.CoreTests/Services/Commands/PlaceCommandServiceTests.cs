using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using ToyRobot.Common.Model;
using ToyRobot.Core.Tests;

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
        var placeCommandService = new PlaceCommandService(
            mock.CoreFactoryService,
            mock.Logger.Object,
            mock.ApplicationMessageService);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(parsed, placeCommandService.TryParse(parts));
    }

    [TestMethod()]
    [DataRow("PLACE,5,5,NORTH")]
    public async Task ExecuteTest(string command)
    {
        var mock = new MockServicesHelper<PlaceCommandService>();
        mock.ActivePlayerSetupProperty(1)
            .ActiveMapSetupProperty(1)
            .ActiveRobotSetupProperty(1);
        var placeCommandService = new PlaceCommandService(
            mock.CoreFactoryService,
            mock.Logger.Object,
            mock.ApplicationMessageService);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, placeCommandService.TryParse(parts));

        var result = await placeCommandService.Execute(mock.Scenario.Object);

        Assert.AreEqual(true, result);
        Assert.AreEqual(1, mock.SetPositionCalled);
        Assert.AreEqual(placeCommandService.CommandResult, CommandResultEnum.Ok);
    }
    [TestMethod()]
    [DataRow("PLACE,5,5,NORTH")]
    public async Task ExecuteTest_NoActiveRobot(string command)
    {
        var mock = new MockServicesHelper<PlaceCommandService>();
        mock.ActivePlayerSetupProperty(1)
            .ActiveMapSetupProperty(1);
        var placeCommandService = new PlaceCommandService(
            mock.CoreFactoryService,
            mock.Logger.Object,
            mock.ApplicationMessageService);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, placeCommandService.TryParse(parts));

        var result = await placeCommandService.Execute(mock.Scenario.Object);

        Assert.AreEqual(false, result);
        Assert.AreEqual(0, mock.SetPositionCalled);
        Assert.AreEqual(placeCommandService.CommandResult, CommandResultEnum.ActiveRobotNull);
    }
    [TestMethod()]
    [DataRow("PLACE,5,5,NORTH")]
    public async Task ExecuteTest_RobotOutOfMap(string command)
    {
        var mock = new MockServicesHelper<PlaceCommandService>();
        mock.ActivePlayerSetupProperty(1)
            .ActiveMapSetupProperty(1, false)
            .ActiveRobotSetupProperty(1);
        var placeCommandService = new PlaceCommandService(
            mock.CoreFactoryService,
            mock.Logger.Object,
            mock.ApplicationMessageService);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, placeCommandService.TryParse(parts));

        var result = await placeCommandService.Execute(mock.Scenario.Object);

        Assert.AreEqual(false, result);
        Assert.AreEqual(0, mock.SetPositionCalled);
        Assert.AreEqual(placeCommandService.CommandResult, CommandResultEnum.RobotCannotMoveOutsideMap);
    }
}
