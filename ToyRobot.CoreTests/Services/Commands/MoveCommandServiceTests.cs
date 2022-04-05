using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using ToyRobot.Common.Model;
using ToyRobot.Core.Tests;

namespace ToyRobot.Core.Services.Commands.Tests;

[TestClass()]
public class MoveCommandServiceTests
{
    [TestMethod()]
    [DataRow("MOVE", true)]
    [DataRow("MOVE,", false)]
    [DataRow("MOV", false)]
    public void TryParseTest(string command, bool parsed)
    {
        var mock = new MockServicesHelper<MoveCommandService>();
        var moveCommandService = new MoveCommandService(
            mock.Logger.Object,
            mock.ApplicationMessageService);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(parsed, moveCommandService.TryParse(parts));
    }

    [TestMethod()]
    [DataRow("MOVE")]
    public async Task ExecuteTest(string command)
    {
        var mock = new MockServicesHelper<MoveCommandService>();
        mock.ActivePlayerSetupProperty(1)
            .ActiveMapSetupProperty(1)
            .ActiveRobotSetupProperty(1)
            .SetActiveRobotPosition(1, 1, Common.Model.MapOrientationEnum.NORTH);
        var moveCommandService = new MoveCommandService(
            mock.Logger.Object,
            mock.ApplicationMessageService);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, moveCommandService.TryParse(parts));

        var result = await moveCommandService.Execute(mock.Scenario.Object);

        Assert.AreEqual(true, result);
        Assert.AreEqual(1, mock.SetPositionCalled);
        Assert.AreEqual(moveCommandService.CommandResult, CommandResultEnum.Ok);
    }
    [TestMethod()]
    [DataRow("MOVE")]
    public async Task ExecuteTest_NoActiveRobot(string command)
    {
        var mock = new MockServicesHelper<MoveCommandService>();
        mock.ActivePlayerSetupProperty(1)
            .ActiveMapSetupProperty(1);
        var moveCommandService = new MoveCommandService(
            mock.Logger.Object,
            mock.ApplicationMessageService);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, moveCommandService.TryParse(parts));

        var result = await moveCommandService.Execute(mock.Scenario.Object);

        Assert.AreEqual(false, result);
        Assert.AreEqual(0, mock.SetPositionCalled);
        Assert.AreEqual(moveCommandService.CommandResult, CommandResultEnum.ActiveRobotNull);
    }
    [TestMethod()]
    [DataRow("MOVE")]
    public async Task ExecuteTest_OutOfMapCurrentPosition(string command)
    {
        var mock = new MockServicesHelper<MoveCommandService>();
        mock.ActivePlayerSetupProperty(1)
            .ActiveMapSetupProperty(1)
            .ActiveRobotSetupProperty(1)
            .SetActiveRobotPosition(null,null,null);
        var moveCommandService = new MoveCommandService(
            mock.Logger.Object,
            mock.ApplicationMessageService);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, moveCommandService.TryParse(parts));

        var result = await moveCommandService.Execute(mock.Scenario.Object);

        Assert.AreEqual(false, result);
        Assert.AreEqual(0, mock.SetPositionCalled);
        Assert.AreEqual(moveCommandService.CommandResult, CommandResultEnum.RobotPositionNull);
    }
    [TestMethod()]
    [DataRow("MOVE")]
    public async Task ExecuteTest_OutOfMapDestination(string command)
    {
        var mock = new MockServicesHelper<MoveCommandService>();
        mock.ActivePlayerSetupProperty(1)
            .ActiveMapSetupProperty(1, false)
            .ActiveRobotSetupProperty(1)
            .SetActiveRobotPosition(1, 1, Common.Model.MapOrientationEnum.NORTH);
        var moveCommandService = new MoveCommandService(
            mock.Logger.Object,
            mock.ApplicationMessageService);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, moveCommandService.TryParse(parts));

        var result = await moveCommandService.Execute(mock.Scenario.Object);

        Assert.AreEqual(true, result);
        Assert.AreEqual(0, mock.SetPositionCalled);
        Assert.AreEqual(moveCommandService.CommandResult, CommandResultEnum.RobotCannotMoveOutsideMap);
    }
}
