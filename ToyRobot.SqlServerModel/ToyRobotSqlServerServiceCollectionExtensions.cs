using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ToyRobot.Common.Services;
using ToyRobot.SqlServerModel.DB;
using ToyRobot.SqlServerModel.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ToyRobotSqlServerServicesCollectionExtensions
{
    public static IServiceCollection AddToyRobotSqlServerServices(
         this IServiceCollection services, IConfiguration config, bool useTestDatabase=false)
    {
        var connectionString = useTestDatabase ?
            "ConnectionStrings:SqlServerTest" : "ConnectionStrings:SqlServer";
        services.AddScoped<IFactoryService, FactorySqlServerDBService>();
        services.AddScoped<IMapService, MapSqlServerDBService>();
        services.AddScoped<IPlayerService, PlayerSqlServerDBService>();

        services.AddScoped<RobotSqlServerDBService>();
        services.AddScoped<IRobotServiceDB>(services => services.GetRequiredService<RobotSqlServerDBService>());
        services.AddScoped<IRobotService>(services => services.GetRequiredService<RobotSqlServerDBService>());

        services.AddScoped<IRobotStepHistoryService, RobotStepSqlServerDBService>();
        services.AddDbContext<ToyRobotDbContext>(options =>
        {
            options.UseSqlServer(config.GetSection(connectionString).Value);
        }
        );
       
        return services;
    }
}
