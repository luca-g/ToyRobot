using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ToyRobot.Services;

namespace ToyRobot.Console;
using Console = System.Console;

internal class ConsoleService : IHostedService
{
    private readonly RobotService _robot;
    private readonly ILogger<ConsoleService> _logger;

    public ConsoleService(ILogger<ConsoleService> logger, RobotService robot)
    {
        this._robot = robot;
        this._logger = logger;
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("ConsoleService StartAsync");
        this.ReadCommands(cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("ConsoleService StopAsync");
        return Task.CompletedTask;
    }
    public void ReadCommands(CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine("ToyRobot");
            Console.WriteLine("Valid commands:");
            Console.WriteLine("PLACE x,y,direction (NORTH,EAST,SOUTH,WEST)");
            Console.WriteLine("MOVE");
            Console.WriteLine("LEFT");
            Console.WriteLine("RIGHT");
            Console.WriteLine("REPORT");
            Console.WriteLine("");

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Waiting for command");
                    string? command = Console.ReadLine();
                    if (command != null)
                    {
                        string? result;
                        if (!_robot.Execute(command, out result))
                        {
                            Console.WriteLine("Invalid command");
                        }
                        else if (result != null)
                        {
                            Console.WriteLine(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error");
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
            _logger.LogError(ex, "Unexpected Error");
            Console.WriteLine("Unexpected error: ", ex.Message);
        }
    }
}
