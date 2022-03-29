using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using ToyRobot.Core.Configuration;
using ToyRobot.SqlServerModel.DB;

namespace ToyRobot.SqlServerModel.Services.Tests;

public abstract class BaseServiceTest
{
    //protected IHost? host;
    public BaseServiceTest()
    {
        //this.CreateHostBuilder();
    }
    //private void CreateHostBuilder()
    //{
    //    var builder = new HostBuilder()
    //         .ConfigureAppConfiguration((configApp) =>
    //         {
    //             configApp.SetBasePath(System.IO.Directory.GetCurrentDirectory());
    //             configApp.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
    //         })
    //         .ConfigureServices((hostContext, services) =>
    //         {
    //             services.Configure<MapSettings>(hostContext.Configuration.GetSection(MapSettings.SectionName));

    //             //services.AddCommandServicesAndConfig(hostContext.Configuration);
    //             //services.AddScoped<IMapService, MapSqlServerDBService>();
    //             //services.AddScoped<IPlayerService, PlayerSqlServerDBService>();
    //             //services.AddScoped<IRobotService, RobotSqlServerDBService>();
    //             //services.AddScoped<IRobotStepHistoryService, RobotStepSqlServerDBService>();
    //             services.AddDbContext<ToyRobotDbContext>(options =>
    //             {
    //                 options.UseSqlServer(hostContext.Configuration.GetSection("ConnectionStrings:SqlServerTest").Value);
    //             }
    //             );
    //             //services.AddLogging(loggingBuilder =>
    //             //{
    //             //    loggingBuilder.ClearProviders();
    //             //    loggingBuilder.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
    //             //    loggingBuilder.AddNLog();
    //             //});
    //         });
    //    this.host = builder.Build();
    //}
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
    protected static DB.Player CreatePlayer(ToyRobotDbContext context)
    {
        var player = new DB.Player { CreationDate = System.DateTime.UtcNow, Identifier = Guid.NewGuid() };    
        context.Player.Add(player);
        context.SaveChanges();
        return player;
    }
    protected static DB.Map CreateMap(ToyRobotDbContext context)
    {
        var map = new DB.Map { CreationDate = System.DateTime.UtcNow, Width = 5, Height = 5 };
        context.Map.Add(map);
        context.SaveChanges();
        return map;
    }
    protected static DB.Robot CreateRobot(ToyRobotDbContext context, int playerId, int mapId)
    {
        var robot = new DB.Robot { CreationDate = System.DateTime.UtcNow, MapId = mapId, PlayerId = playerId };
        context.Robot.Add(robot);
        context.SaveChanges();
        return robot;
    }
}
