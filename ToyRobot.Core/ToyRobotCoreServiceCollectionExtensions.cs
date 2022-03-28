using Microsoft.Extensions.Configuration;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;
using ToyRobot.Core.Configuration;
using ToyRobot.Core.Services;
using ToyRobot.Core.Services.Commands;

namespace Microsoft.Extensions.DependencyInjection;

public static class ToyRobotCoreServiceCollectionExtensions
{
    public static IServiceCollection AddCommandServicesAndConfig(
         this IServiceCollection services, IConfiguration config)
    {
        services.Configure<MapSettings>(config.GetSection(MapSettings.SectionName));
        services.AddSingleton<IApplicationMessagesService, ApplicationMessagesService>();
        services.AddTransient<ICommand, CreateMapCommandService>();
        services.AddTransient<ICommand, LeftCommandService>();
        services.AddTransient<ICommand, MoveCommandService>();
        services.AddTransient<ICommand, PlaceCommandService>();
        services.AddTransient<ICommand, ReportCommandService>();
        services.AddTransient<ICommand, RightCommandService>();
        services.AddTransient<ICommand, SizeCommandService>();
        services.AddTransient<ICommand, CreateRobotCommandService>();
        services.AddTransient<ICommandCenterService, CommandCenterService>();
        return services;
    }
}
