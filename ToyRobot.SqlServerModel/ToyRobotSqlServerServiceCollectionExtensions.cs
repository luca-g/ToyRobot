using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ToyRobot.Common.Services;
using ToyRobot.SqlServerModel.DB;
using ToyRobot.SqlServerModel.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ToyRobotSqlServerServicesCollectionExtensions
{
    public static IServiceCollection AddToyRobotSqlServerServices(
         this IServiceCollection services, HostBuilderContext hostContext)
    {
        services.AddScoped<IMapService, MapSqlServerDBService>();
        services.AddScoped<IPlayerService, PlayerSqlServerDBService>();
        services.AddScoped<IRobotService, RobotSqlServerDBService>();
        services.AddScoped<IRobotStepHistoryService, RobotStepSqlServerDBService>();
        services.AddDbContext<ToyRobotDbContext>(options =>
        {
            options.UseSqlServer(hostContext.Configuration.GetSection("ConnectionStrings:SqlServer").Value);
        }
        );
       ;
        return services;
    }
}
