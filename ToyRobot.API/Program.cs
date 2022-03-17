using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace ToyRobot.API;
class Program
{
    static async Task Main(string[] args)
    {
        var logger = NLog.LogManager.GetCurrentClassLogger();
        logger.Info("Starting");
        try
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost
                .ConfigureAppConfiguration((configApp) =>
                {
                    configApp.SetBasePath(System.IO.Directory.GetCurrentDirectory());
                    configApp.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddRobotServiceConfig(hostContext.Configuration);
                    services.AddLogging(loggingBuilder =>
                    {
                        loggingBuilder.ClearProviders();
                        loggingBuilder.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                        loggingBuilder.AddNLog();
                    });
                });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
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
