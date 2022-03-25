using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;

namespace ToyRobot.Console;
using Console = System.Console;

internal class ConsoleService : IHostedService
{
    private readonly IRobotService robotService;
    private readonly ILogger<ConsoleService> loggerService;
    private readonly ICommandCenterService commandCenterService;
    private readonly IPlayerService playerService;
    private readonly IMapService mapService;
    public ConsoleService(
        ILogger<ConsoleService> logger, 
        IRobotService robot, 
        ICommandCenterService commandCenterService,
        IPlayerService playerService,
        IMapService mapService)
    {
        this.robotService = robot;
        this.loggerService = logger;
        this.commandCenterService = commandCenterService;
        this.playerService = playerService;
        this.mapService = mapService;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        loggerService.LogInformation("ConsoleService StartAsync");
        await this.ReadCommands(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        loggerService.LogInformation("ConsoleService StopAsync");
        return Task.CompletedTask;
    }
    public async Task ReadCommands(CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine("ToyRobot");
            Console.WriteLine("Valid commands:");
            foreach(var command in commandCenterService.Commands)
            {
                Console.WriteLine(command.ConsoleInstruction);
            }
            Console.WriteLine("");

            var player = await playerService.CreatePlayer();
            mapService.ActiveMap = await mapService.CreateMap(mapService.MapSettings.MinWidth, mapService.MapSettings.MinHeight);
            robotService.ActiveRobot = await robotService.CreateRobot(player.PlayerId, mapService.ActiveMap.MapId);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    loggerService.LogInformation("Waiting for command");
                    string? command = Console.ReadLine();
                    if (command != null)
                    {
                        await commandCenterService.Execute(command);
                        Console.WriteLine(commandCenterService.ExecuteResult ?? "Invalid command");
                    }
                }
                catch (Exception ex)
                {
                    loggerService.LogError(ex, "Error");
                    Console.WriteLine(ex.Message);
                    if (cancellationToken.IsCancellationRequested)
                        return;
                }
            }
        }
        catch (Exception ex)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            loggerService.LogError(ex, "Unexpected Error");
            Console.WriteLine("Unexpected error: {0}", ex.Message);
        }
    }
}
