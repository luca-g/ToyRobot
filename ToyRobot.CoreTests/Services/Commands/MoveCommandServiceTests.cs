﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using ToyRobot.MockHelper;


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
        var leftCommandService = new MoveCommandService(
            mock.Logger.Object,
            mock.RobotService.Object);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(parsed, leftCommandService.TryParse(parts));
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
        var leftCommandService = new MoveCommandService(
            mock.Logger.Object,
            mock.RobotService.Object);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, leftCommandService.TryParse(parts));

        var result = await leftCommandService.Execute();

        Assert.AreEqual(true, result);
    }
}