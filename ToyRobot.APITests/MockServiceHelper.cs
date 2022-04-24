using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Diagnostics;
using System.Security.Claims;
using ToyRobot.Common.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;




namespace ToyRobot.APITests;
public class MockServicesHelper<T>
{
    public Mock<IScenario> Scenario = new();
    public Mock<ILogger<T>> Logger = new();
    public Mock<IHttpContextAccessor> HttpContextAccessor = new();

    public void SetUserGuid(string? guid = null)
    {
        HttpContextAccessor = GetHttpContextAccessor(guid);
    }
    private static Mock<IHttpContextAccessor> GetHttpContextAccessor(string? userId = null)
    {
        if(userId == null)
        {
            userId = Guid.NewGuid().ToString();
        }

        var user = new ClaimsPrincipal(
            new ClaimsIdentity(
                new Claim[] { new Claim("userGuid", userId) },
                "Basic")
            );

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        httpContextAccessorMock.Setup(h => h.HttpContext.User).Returns(user);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        return httpContextAccessorMock;
    }

    public IHost GetHost()
    {
        var host = new HostBuilder()
             .ConfigureAppConfiguration((configApp) =>
             {
                 configApp.SetBasePath(System.IO.Directory.GetCurrentDirectory());
                 configApp.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
                 configApp.AddJsonFile("Language/ApplicationMessages.en.json", optional: false, reloadOnChange: false);
             })
             .ConfigureServices((hostContext, services) =>
             {
                 services.AddCommandServicesAndConfig(hostContext.Configuration);
                 services.AddToyRobotSqlServerServices(hostContext.Configuration, true);
             })
             .Build();
        return host;
    }
    /*
    public Mock<IPlayerService> PlayerService = new();
    public Mock<IRobotService> RobotService = new();
    public Mock<IOptions<MapSettings>> MapSettingsOptions = new();

    public int SetPositionCalled = 0;

    public MockServicesHelper<T> ActiveRobotSetupProperty(int? value)
    {
        this.Scenario
            .SetupGet(t => t.IsRobotSet).Returns(true);
        this.Scenario
            .SetupGet(t => t.IsRobotDeployed).Returns(true);
        if (value.HasValue)
        {
            this.Scenario
                .SetupGet(t => t.RobotId).Returns(value);
            this.Scenario
                .Setup(t => t.SetMapPosition(It.IsAny<IMapPosition>()))
                .Callback(() => SetPositionCalled++);
        }
        return this;
    }
    public MockServicesHelper<T> ActivePlayerSetupProperty(int? value)
    {
        if (value.HasValue)
        {
            this.Scenario
                .SetupGet(t => t.PlayerId).Returns(value.Value);
        }
        return this;
    }
    */
}
