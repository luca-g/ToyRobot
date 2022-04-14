using ToyRobot.SqlServerModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using ToyRobot.SqlServerModelTests;
using System;
using System.Collections.Generic;

namespace ToyRobot.SqlServerModel.Services.Tests;

[TestClass()]
public class PlayerSqlServerDBServiceTests : BaseServiceTest
{
    [TestMethod()]
    public async Task CreatePlayerTestAsync()
    {
        var mock = new MockServicesHelper<PlayerSqlServerDBService>();
        using var context = Context();
        var playerSqlServerDBService = new PlayerSqlServerDBService(
            mock.Logger.Object,
            context);
        var player = await playerSqlServerDBService.CreatePlayer();
        Assert.IsNotNull(player);
        Assert.IsTrue(player.PlayerId > 0);
    }

    [TestMethod()]
    public async Task LoadPlayerTestAsync()
    {
        var mock = new MockServicesHelper<PlayerSqlServerDBService>();
        using var context = Context();
        var playerSqlServerDBService = new PlayerSqlServerDBService(
            mock.Logger.Object,
            context);
        var player = await playerSqlServerDBService.CreatePlayer();
        Assert.IsNotNull(player);
        Assert.IsTrue(player.PlayerId > 0);

        var loadPlayer = await playerSqlServerDBService.LoadPlayer(player.PlayerGuid);
        Assert.IsNotNull(loadPlayer);
        Assert.IsTrue(player.PlayerId == loadPlayer.PlayerId);
    }
    [TestMethod()]
    public async Task LoadPlayerTestAsync_PlayerNotFound()
    {
        var mock = new MockServicesHelper<PlayerSqlServerDBService>();
        using var context = Context();
        var playerSqlServerDBService = new PlayerSqlServerDBService(
            mock.Logger.Object,
            context);
        var guid = System.Guid.NewGuid();
        try
        {
            var loadPlayer = await playerSqlServerDBService.LoadPlayer(guid);
        }
        catch(Exception ex)
        {
            Assert.IsTrue(ex is KeyNotFoundException);
            return;
        }
        Assert.Fail();
    }
}
