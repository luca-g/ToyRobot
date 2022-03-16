﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using ToyRobot.Configuration;

namespace ToyRobot.Console;

class Program
{
    static async Task Main(string[] args)
    {
        var logger = NLog.LogManager.GetCurrentClassLogger();
        logger.Info("Starting");
        try
        {
            var host = new HostBuilder()
                 .ConfigureAppConfiguration((configApp) =>
                 {
                     configApp.SetBasePath(System.IO.Directory.GetCurrentDirectory());
                     configApp.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
                 })
                 .ConfigureServices((hostContext, services) =>
                 {
                     services.AddRobotServiceConfig(hostContext.Configuration);
                     services.AddHostedService<ConsoleService>();
                     services.AddLogging(loggingBuilder =>
                     {
                         loggingBuilder.ClearProviders();
                         loggingBuilder.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                         loggingBuilder.AddNLog();
                     });
                 });
            await host.Build().RunAsync();
        }
        catch (Exception ex)
        {
            logger.Error(ex);
        }
        finally
        {
            logger.Info("Exiting program");
            NLog.LogManager.Shutdown();
        }

    }
}

