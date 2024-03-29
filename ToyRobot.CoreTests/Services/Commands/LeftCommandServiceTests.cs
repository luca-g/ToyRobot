﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using ToyRobot.Common.Model;
using ToyRobot.Core.Tests;

namespace ToyRobot.Core.Services.Commands.Tests;

[TestClass()]
public class LeftCommandServiceTests
{
    [TestMethod()]
    [DataRow("LEFT", true)]
    [DataRow("LEFT,", false)]
    [DataRow("LEF", false)]
    public void TryParseTest(string command, bool parsed)
    {
        var mock = new MockServicesHelper<LeftCommandService>();
        var leftCommandService = new LeftCommandService(
            mock.CoreFactoryService,
            mock.Logger.Object,
            mock.ApplicationMessageService);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(parsed, leftCommandService.TryParse(parts));
    }

    [TestMethod()]
    [DataRow("LEFT")]
    public async Task ExecuteTest(string command)
    {
        var mock = new MockServicesHelper<LeftCommandService>();
        mock.ActivePlayerSetupProperty(1)
            .ActiveMapSetupProperty(1)
            .ActiveRobotSetupProperty(1)
            .SetActiveRobotPosition(1,1,Common.Model.MapOrientationEnum.NORTH);
        var leftCommandService = new LeftCommandService(
            mock.CoreFactoryService,
            mock.Logger.Object,
            mock.ApplicationMessageService);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, leftCommandService.TryParse(parts));

        var result = await leftCommandService.Execute(mock.Scenario.Object);

        Assert.AreEqual(true, result);
        Assert.AreEqual(1, mock.SetPositionCalled);
        Assert.AreEqual(leftCommandService.CommandResult, CommandResultEnum.Ok);
    }
    [TestMethod()]
    [DataRow("LEFT")]
    public async Task ExecuteTest_NoActiveRobot(string command)
    {
        var mock = new MockServicesHelper<LeftCommandService>();
        mock.ActivePlayerSetupProperty(1)
            .ActiveMapSetupProperty(1);
        var leftCommandService = new LeftCommandService(
            mock.CoreFactoryService,
            mock.Logger.Object,
            mock.ApplicationMessageService);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, leftCommandService.TryParse(parts));

        var result = await leftCommandService.Execute(mock.Scenario.Object);

        Assert.AreEqual(false, result);
        Assert.AreEqual(0, mock.SetPositionCalled);
        Assert.AreEqual(leftCommandService.CommandResult, CommandResultEnum.ActiveRobotNull);
    }
    [TestMethod()]
    [DataRow("LEFT")]
    public async Task ExecuteTest_RobotOutOfMap(string command)
    {
        var mock = new MockServicesHelper<LeftCommandService>();
        mock.ActivePlayerSetupProperty(1)
            .ActiveMapSetupProperty(1)
            .ActiveRobotSetupProperty(1)
            .SetActiveRobotPosition(null,null,null);
        var leftCommandService = new LeftCommandService(
            mock.CoreFactoryService,
            mock.Logger.Object,
            mock.ApplicationMessageService);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, leftCommandService.TryParse(parts));

        var result = await leftCommandService.Execute(mock.Scenario.Object);

        Assert.AreEqual(false, result);
        Assert.AreEqual(0, mock.SetPositionCalled);
        Assert.AreEqual(leftCommandService.CommandResult, CommandResultEnum.RobotPositionNull);
    }
}
