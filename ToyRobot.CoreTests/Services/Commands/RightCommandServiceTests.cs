using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using ToyRobot.MockHelper;

namespace ToyRobot.Core.Services.Commands.Tests;

[TestClass()]
public class RightCommandServiceTests
{
    [TestMethod()]
    [DataRow("RIGHT", true)]
    [DataRow("RIGHT,", false)]
    [DataRow("RIGH", false)]
    public void TryParseTest(string command, bool parsed)
    {
        var mock = new MockServicesHelper<RightCommandService>();
        var rightCommandService = new RightCommandService(
            mock.Logger.Object,
            mock.RobotService.Object);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(parsed, rightCommandService.TryParse(parts));
    }

    [TestMethod()]
    [DataRow("RIGHT")]
    public async Task ExecuteTest(string command)
    {
        var mock = new MockServicesHelper<RightCommandService>();
        mock.ActivePlayerSetupProperty(1)
            .ActiveMapSetupProperty(1)
            .ActiveRobotSetupProperty(1)
            .SetActiveRobotPosition(1, 1, Common.Model.MapOrientationEnum.NORTH);
        var rightCommandService = new RightCommandService(
            mock.Logger.Object,
            mock.RobotService.Object);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, rightCommandService.TryParse(parts));

        var result = await rightCommandService.Execute();

        Assert.AreEqual(true, result);
        Assert.AreEqual(1, mock.SetPositionCalled);
    }
    [TestMethod()]
    [DataRow("RIGHT")]
    public async Task ExecuteTest_NoActiveRobot(string command)
    {
        var mock = new MockServicesHelper<RightCommandService>();
        mock.ActivePlayerSetupProperty(1)
            .ActiveMapSetupProperty(1);
        var rightCommandService = new RightCommandService(
            mock.Logger.Object,
            mock.RobotService.Object);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, rightCommandService.TryParse(parts));

        var result = await rightCommandService.Execute();

        Assert.AreEqual(false, result);
        Assert.AreEqual(0, mock.SetPositionCalled);
    }
    [TestMethod()]
    [DataRow("RIGHT")]
    public async Task ExecuteTest_RobotOutOfMap(string command)
    {
        var mock = new MockServicesHelper<RightCommandService>();
        mock.ActivePlayerSetupProperty(1)
            .ActiveMapSetupProperty(1)
            .ActiveRobotSetupProperty(1)
            .SetActiveRobotPosition(null, null, null);
        var rightCommandService = new RightCommandService(
            mock.Logger.Object,
            mock.RobotService.Object);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, rightCommandService.TryParse(parts));

        var result = await rightCommandService.Execute();

        Assert.AreEqual(false, result);
        Assert.AreEqual(0, mock.SetPositionCalled);
    }
}
