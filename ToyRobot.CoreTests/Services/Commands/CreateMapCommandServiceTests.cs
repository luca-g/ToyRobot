﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using ToyRobot.Common.Model;
using ToyRobot.Core.Tests;

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
            mock.CoreFactoryService,
            mock.Logger.Object,
            mock.RobotStepHistoryService.Object,
            mock.MapService.Object,
            mock.ApplicationMessageService);

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
            mock.CoreFactoryService,
            mock.Logger.Object,
            mock.RobotStepHistoryService.Object,
            mock.MapService.Object,
            mock.ApplicationMessageService);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, mapCommandService.TryParse(parts));

        var result = await mapCommandService.Execute(mock.Scenario.Object);

        Assert.AreEqual(true, result);
        Assert.AreEqual(1, mock.SetActiveMapCalled);
        Assert.IsTrue(mapCommandService.CommandResult == CommandResultEnum.Ok);
    }
    [TestMethod()]
    [DataRow("CREATEMAP,5,5")]
    public async Task ExecuteTest_NoMapCreated(string command)
    {
        var mock = new MockServicesHelper<CreateMapCommandService>();
        var mapCommandService = new CreateMapCommandService(
            mock.CoreFactoryService,
            mock.Logger.Object,
            mock.RobotStepHistoryService.Object,
            mock.MapService.Object,
            mock.ApplicationMessageService);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, mapCommandService.TryParse(parts));

        var result = await mapCommandService.Execute(mock.Scenario.Object);

        Assert.AreEqual(false, result);
        Assert.IsTrue(mapCommandService.CommandResult == CommandResultEnum.MapCreateError);
    }
}
