using ToyRobot.SqlServerModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using ToyRobot.SqlServerModelTests;

namespace ToyRobot.SqlServerModel.Services.Tests;

[TestClass()]
public class RobotSqlServerDBServiceTests : BaseServiceTest
{
    [TestMethod()]
    public async Task CreateRobotTestAsync()
    {
        var mock = new MockServicesHelper<RobotSqlServerDBService>();
        using var context = Context();
        var player = CreatePlayer(context);
        var map = CreateMap(context);
        var robotSqlServerDBService = new RobotSqlServerDBService(
            mock.Logger.Object,
            context);
        var robot = await robotSqlServerDBService.CreateRobot(player.PlayerId, map.MapId);
        Assert.IsNotNull(robot);
        Assert.IsTrue(robot.RobotId > 0);
    }

    [TestMethod()]
    public async Task LoadRobotsTestAsync()
    {
        var mock = new MockServicesHelper<RobotSqlServerDBService>();
        using var context = Context();
        var player = CreatePlayer(context);
        var map = CreateMap(context);
        var robotSqlServerDBService = new RobotSqlServerDBService(
            mock.Logger.Object,
            context);
        var robot = await robotSqlServerDBService.CreateRobot(player.PlayerId, map.MapId);
        Assert.IsNotNull(robot);
        Assert.IsTrue(robot.RobotId > 0);

        map = CreateMap(context);
        robot = await robotSqlServerDBService.CreateRobot(player.PlayerId, map.MapId);
        Assert.IsNotNull(robot);
        Assert.IsTrue(robot.RobotId > 0);

        var robots = await robotSqlServerDBService.LoadRobots(player.PlayerId, map.MapId);
        Assert.IsNotNull(robots);
        Assert.IsTrue(robots.Count == 1);
        Assert.IsTrue(robots[0].RobotId == robot.RobotId);

        var robotsAllMaps = await robotSqlServerDBService.LoadRobots(player.PlayerId, null);
        Assert.IsNotNull(robotsAllMaps);
        Assert.IsTrue(robotsAllMaps.Count == 2);
    }
}
