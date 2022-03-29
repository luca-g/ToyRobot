using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Diagnostics;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;
using ToyRobot.Core.Configuration;

namespace ToyRobot.SqlServerModelTests;
public class MockServicesHelper<T>
{
    public Mock<ILogger<T>> Logger = new();
    public Mock<IPlayerService> PlayerService = new();
    public Mock<IRobotService> RobotService = new();
    public Mock<IOptions<MapSettings>> MapSettingsOptions = new();
    Mock<IRobot>? mockRobot = null;
    Mock<IPlayer>? mockPlayer = null;
    
    public MockServicesHelper<T> ActiveRobotSetupProperty(int? value)
    {
        if (value.HasValue)
        {
            Debug.Assert(mockPlayer != null);

            mockRobot = new();
            mockRobot.SetupGet(t => t.RobotId).Returns(value.Value);
            mockRobot.SetupGet(t => t.Player).Returns(mockPlayer.Object);
        }
        else
            mockRobot = null;

        this.RobotService
            .SetupProperty(t => t.ActiveRobot, mockRobot?.Object);
        return this;
    }
    public MockServicesHelper<T> ActivePlayerSetupProperty(int? value)
    {
        if (value.HasValue)
        {
            if (mockPlayer == null)
            {
                mockPlayer = new Mock<IPlayer>();
            }
            mockPlayer.SetupGet(t => t.PlayerId).Returns(value.Value);
        }
        this.PlayerService
            .SetupProperty(t => t.ActivePlayer, mockPlayer?.Object);
        return this;
    }
}
