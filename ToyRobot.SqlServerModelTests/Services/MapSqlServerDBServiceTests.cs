using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using ToyRobot.SqlServerModelTests;

namespace ToyRobot.SqlServerModel.Services.Tests;

[TestClass()]
public class MapSqlServerDBServiceTests : BaseServiceTest
{
    public MapSqlServerDBServiceTests() : base() { }
    [TestMethod()]
    public async Task CreateMapTest()
    {
        var mock = new MockServicesHelper<MapSqlServerDBService>();
        using var context = Context();
        var mapSqlServerDBService = new MapSqlServerDBService(
            mock.Logger.Object,
            context,
            mock.MapSettingsOptions.Object,
            mock.PlayerService.Object);
        var map = await mapSqlServerDBService.CreateMap(5, 5);
        Assert.IsNotNull(map);
    }
    [TestMethod()]
    public async Task LoadMapsTestAsync()
    {
        using var context = Context();
        var player = CreatePlayer(context);
        var map = CreateMap(context);
        var robot= CreateRobot(context,player.PlayerId, map.MapId);

        var mock = new MockServicesHelper<MapSqlServerDBService>();
        mock.ActivePlayerSetupProperty(player.PlayerId);
        var mapSqlServerDBService = new MapSqlServerDBService(
            mock.Logger.Object,
            context,
            mock.MapSettingsOptions.Object,
            mock.PlayerService.Object);
        var maps = await mapSqlServerDBService.LoadMaps(player.PlayerId);
        Assert.IsNotNull(maps);
        Assert.IsTrue(maps.Count==1);
    }
}
