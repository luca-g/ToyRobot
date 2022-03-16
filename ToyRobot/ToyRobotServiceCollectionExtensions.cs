using Microsoft.Extensions.Configuration;
using ToyRobot.Configuration;
using ToyRobot.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ToyRobotServiceCollectionExtensions
{
    public static IServiceCollection AddRobotServiceConfig(
         this IServiceCollection services, IConfiguration config)
    {
        services.Configure<MapSettings>(config.GetSection(MapSettings.SectionName));
        services.AddTransient<MapService>();
        services.AddTransient<RobotService>();
        return services;
    }
}
