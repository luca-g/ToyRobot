using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using ToyRobot.Common.Model;
using ToyRobot.Core.Tests;

namespace ToyRobot.Core.Services.Commands.Tests;

[TestClass()]
public class SizeCommandServiceTests
{
    [TestMethod()]
    [DataRow("SIZE", true)]
    [DataRow("SIZE,", false)]
    [DataRow("SIZ", false)]
    public void TryParseTest(string command, bool parsed)
    {
        var mock = new MockServicesHelper<SizeCommandService>();
        var sizeCommandService = new SizeCommandService(
            mock.Logger.Object,
            mock.MapService.Object,
            mock.ApplicationMessageService);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(parsed, sizeCommandService.TryParse(parts));
    }

    [TestMethod()]
    [DataRow("SIZE")]
    public async Task ExecuteTest(string command)
    {
        var mock = new MockServicesHelper<SizeCommandService>();
        mock.ActivePlayerSetupProperty(1)
            .ActiveMapSetupProperty(1);
        var sizeCommandService = new SizeCommandService(
            mock.Logger.Object,
            mock.MapService.Object,
            mock.ApplicationMessageService);


        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, sizeCommandService.TryParse(parts));

        var result = await sizeCommandService.Execute();

        Assert.AreEqual(true, result);
        Assert.AreEqual(sizeCommandService.CommandResult, CommandResultEnum.Ok);
    }
    [TestMethod()]
    [DataRow("SIZE")]
    public async Task ExecuteTest_NoActiveMap(string command)
    {
        var mock = new MockServicesHelper<SizeCommandService>();
        mock.ActivePlayerSetupProperty(1);
        var sizeCommandService = new SizeCommandService(
            mock.Logger.Object,
            mock.MapService.Object,
            mock.ApplicationMessageService);


        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, sizeCommandService.TryParse(parts));

        var result = await sizeCommandService.Execute();

        Assert.AreEqual(true, result);
        Assert.AreEqual(sizeCommandService.CommandResult, CommandResultEnum.ActiveMapNull);
    }
}
