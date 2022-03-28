using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using ToyRobot.Common.Model;
using ToyRobot.MockHelper;

namespace ToyRobot.Core.Services.Commands.Tests;

[TestClass()]
public class ReportCommandServiceTests
{
    [TestMethod()]
    [DataRow("REPORT", true)]
    [DataRow("REPORT,", false)]
    [DataRow("REPOR", false)]
    public void TryParseTest(string command, bool parsed)
    {
        var mock = new MockServicesHelper<ReportCommandService>();
        var reportCommandService = new ReportCommandService(
            mock.Logger.Object,
            mock.RobotService.Object,
            mock.ApplicationMessageService);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(parsed, reportCommandService.TryParse(parts));
    }

    [TestMethod()]
    [DataRow("REPORT")]
    public async Task ExecuteTest(string command)
    {
        var mock = new MockServicesHelper<ReportCommandService>();
        mock.ActivePlayerSetupProperty(1)
            .ActiveMapSetupProperty(1)
            .ActiveRobotSetupProperty(1);
        var reportCommandService = new ReportCommandService(
            mock.Logger.Object,
            mock.RobotService.Object,
            mock.ApplicationMessageService);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, reportCommandService.TryParse(parts));

        var result = await reportCommandService.Execute();

        Assert.AreEqual(true, result);
    }
    [TestMethod()]
    [DataRow("REPORT")]
    public async Task ExecuteTest_NoActiveRobot(string command)
    {
        var mock = new MockServicesHelper<ReportCommandService>();
        mock.ActivePlayerSetupProperty(1)
            .ActiveMapSetupProperty(1);
        var reportCommandService = new ReportCommandService(
            mock.Logger.Object,
            mock.RobotService.Object,
            mock.ApplicationMessageService);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, reportCommandService.TryParse(parts));

        var result = await reportCommandService.Execute();

        Assert.AreEqual(false, result);
        Assert.AreEqual(reportCommandService.CommandResult, CommandResultEnum.ActiveRobotNull);
    }
    [TestMethod()]
    [DataRow("REPORT")]
    public async Task ExecuteTest_RobotOutOfMap(string command)
    {
        var mock = new MockServicesHelper<ReportCommandService>();
        mock.ActivePlayerSetupProperty(1)
            .ActiveMapSetupProperty(1)
            .ActiveRobotSetupProperty(1)
            .SetActiveRobotPosition(null,null,null);
        var reportCommandService = new ReportCommandService(
            mock.Logger.Object,
            mock.RobotService.Object,
            mock.ApplicationMessageService);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, reportCommandService.TryParse(parts));

        var result = await reportCommandService.Execute();

        Assert.AreEqual(true, result);
        Assert.AreEqual(reportCommandService.CommandResult, CommandResultEnum.RobotPositionNull);
    }
}
