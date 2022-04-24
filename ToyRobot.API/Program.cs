using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using ToyRobot.Common.Services;

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
                    string path = System.IO.Directory.GetCurrentDirectory();
                    logger.Log(NLog.LogLevel.Info, "Api directory " + path);
                    configApp.SetBasePath(path);
                    configApp.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
                    configApp.AddJsonFile("Language/ApplicationMessages.en.json", optional: false, reloadOnChange: false);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging(loggingBuilder =>
                    {
                        loggingBuilder.ClearProviders();
                        loggingBuilder.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                        loggingBuilder.AddNLog();
                    });
                    services.AddHttpContextAccessor();
                    services.AddJwtTokenAuthentication(hostContext.Configuration);
                    services.AddCommandServicesAndConfig(hostContext.Configuration);
                    services.AddToyRobotSqlServerServices(hostContext.Configuration);
                });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
#pragma warning disable CA1825 // Avoid zero-length array allocations
                option.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                      },
                      new string[] { }
                    }
                  });
#pragma warning restore CA1825 // Avoid zero-length array allocations
            });
        
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            ToyRobotServices.Instance.SetServiceProvider(app.Services);
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
