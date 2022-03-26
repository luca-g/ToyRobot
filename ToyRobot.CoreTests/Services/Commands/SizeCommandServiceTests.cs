using ToyRobot.Core.Services.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using ToyRobot.MockHelper;

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
        var reportCommandService = new SizeCommandService(
            mock.Logger.Object,
            mock.MapService.Object);

        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(parsed, reportCommandService.TryParse(parts));
    }

    [TestMethod()]
    [DataRow("SIZE")]
    public async Task ExecuteTest(string command)
    {
        var mock = new MockServicesHelper<SizeCommandService>();
        mock.ActivePlayerSetupProperty(1)
            .ActiveMapSetupProperty(1);
        var reportCommandService = new SizeCommandService(
            mock.Logger.Object,
            mock.MapService.Object);


        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, reportCommandService.TryParse(parts));

        var result = await reportCommandService.Execute();

        Assert.AreEqual(true, result);
    }
    [TestMethod()]
    [DataRow("SIZE")]
    public async Task ExecuteTest_NoActiveMap(string command)
    {
        var mock = new MockServicesHelper<SizeCommandService>();
        mock.ActivePlayerSetupProperty(1);
        var reportCommandService = new SizeCommandService(
            mock.Logger.Object,
            mock.MapService.Object);


        var parts = command.Split(new char[] { ' ', ',' });
        Assert.AreEqual(true, reportCommandService.TryParse(parts));

        var result = await reportCommandService.Execute();

        Assert.AreEqual(true, result);
        Assert.IsTrue(reportCommandService.ExecuteResult?.StartsWith("Map not set"));
    }
}
