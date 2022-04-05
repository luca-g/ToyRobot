using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using ToyRobot.Common.Model;
using ToyRobot.Core.Configuration;
using ToyRobot.SqlServerModel.DB;

namespace ToyRobot.SqlServerModel.Services.Tests;

public abstract class BaseServiceTest
{
    protected static ToyRobotDbContext Context()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(System.IO.Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        var optionsBuilder = new DbContextOptionsBuilder<ToyRobotDbContext>()
            .UseSqlServer(configuration.GetConnectionString("SqlServerTest"));            
        var context = new ToyRobotDbContext(optionsBuilder.Options);
        return context;
    }
    protected static IPlayer CreatePlayer(ToyRobotDbContext context)
    {
        var player = new DB.Player { CreationDate = System.DateTime.UtcNow, Identifier = Guid.NewGuid() };    
        context.Player.Add(player);
        context.SaveChanges();
        return player;
    }
    protected static IMap CreateMap(ToyRobotDbContext context)
    {
        var map = new DB.Map { CreationDate = System.DateTime.UtcNow, Width = 5, Height = 5 };
        context.Map.Add(map);
        context.SaveChanges();
        return map;
    }
    protected static IRobot CreateRobot(ToyRobotDbContext context, int playerId, int mapId)
    {
        var robot = new DB.Robot { CreationDate = System.DateTime.UtcNow, MapId = mapId, PlayerId = playerId };
        context.Robot.Add(robot);
        context.SaveChanges();
        return robot;
    }
}
