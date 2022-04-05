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
    public Mock<IScenario> Scenario = new();
    public Mock<ILogger<T>> Logger = new();
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
}
