using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using ToyRobot.SqlServerModelTests;
using System.Linq;

namespace ToyRobot.SqlServerModel.Services.Tests;

[TestClass()]
public class RobotStepSqlServerDBServiceTests : BaseServiceTest
{
    [TestMethod()]
    public async Task AddStepTestAsync()
    {
        var mock = new MockServicesHelper<RobotStepSqlServerDBService>();
        using var context = Context();
        var player = CreatePlayer(context);
        var map = CreateMap(context);
        var robot = CreateRobot(context, player.PlayerId, map.MapId);
        mock.ActivePlayerSetupProperty(player.PlayerId);
        mock.ActiveRobotSetupProperty(robot.RobotId);

        var robotStepSqlServerDBService = new RobotStepSqlServerDBService(
            mock.Logger.Object,
            context);

        int id;
        try
        {
            id = context.Command.Select(t => t.CommandId).Max();
        }
        catch
        {
            id = 0;
        }
        await robotStepSqlServerDBService.AddStep(
                mock.Scenario.Object,
                "commandTest",
                true
            );
        var readCommand = context.Command
            .Where(t=>t.CommandId > id && t.CommandText=="commandTest")
            .FirstOrDefault();

        Assert.IsNotNull(readCommand);
    }
}
