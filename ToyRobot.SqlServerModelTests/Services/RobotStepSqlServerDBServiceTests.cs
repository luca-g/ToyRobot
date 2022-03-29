using ToyRobot.SqlServerModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using ToyRobot.SqlServerModelTests;
using ToyRobot.Core.Model;
using ToyRobot.Common.Model;
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
            context,
            mock.RobotService.Object);

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
                new MapPosition(1, 1, MapOrientationEnum.NORTH),
                null,
                "commandTest",
                true,
                "Ok"
            );
        var readCommand = context.Command
            .Where(t=>t.CommandId > id && t.CommandText=="commandTest")
            .FirstOrDefault();

        Assert.IsNotNull(readCommand);
    }
}
