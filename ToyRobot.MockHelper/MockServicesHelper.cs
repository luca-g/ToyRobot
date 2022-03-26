using Microsoft.Extensions.Logging;
using Moq;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;

namespace ToyRobot.MockHelper;
public class MockServicesHelper<T>
{
    public Mock<ILogger<T>> Logger = new Mock<ILogger<T>>();
    public Mock<IMapService> MapService = new Mock<IMapService>();
    public Mock<IPlayerService> PlayerService = new Mock<IPlayerService>();
    public Mock<IRobotService> RobotService = new Mock<IRobotService>();
    public Mock<IRobotStepHistoryService> RobotStepHistoryService = new Mock<IRobotStepHistoryService>();

    public MockServicesHelper<T> CreateMapSetup()
    {
        var mockMap = new Mock<IMap>();
        mockMap.SetupGet(t => t.MapId).Returns(1);

        this.MapService
            .Setup(t => t.CreateMap(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(Task.FromResult(mockMap.Object));
        return this;
    }
    public MockServicesHelper<T> CreateRobotSetup()
    {
        var mockRobot = new Mock<IRobot>();
        mockRobot.SetupGet(t => t.RobotId).Returns(1);

        this.RobotService
            .Setup(t => t.CreateRobot(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(Task.FromResult(mockRobot.Object));
        return this;
    }

    public MockServicesHelper<T> ActiveRobotSetupProperty(int? value)
    {
        Mock<IRobot>? mockRobot = null;
        if (value.HasValue)
        {
            mockRobot = new Mock<IRobot>();
            mockRobot.SetupGet(t => t.RobotId).Returns(value.Value);
        }
        this.RobotService
            .SetupProperty(t => t.ActiveRobot, mockRobot?.Object);
        return this;
    }

    public MockServicesHelper<T> ActiveMapSetupProperty(int? value)
    {
        Mock<IMap>? mockMap = null;
        if (value.HasValue)
        {
            mockMap = new Mock<IMap>();
            mockMap.SetupGet(t => t.MapId).Returns(value.Value);
        }
        this.MapService
            .SetupProperty(t => t.ActiveMap, mockMap?.Object);
        return this;
    }
    public MockServicesHelper<T> ActivePlayerSetupProperty(int? value)
    {
        Mock<IPlayer>? mockPlayer = null;
        if (value.HasValue)
        {
            mockPlayer = new Mock<IPlayer>();
            mockPlayer.SetupGet(t => t.PlayerId).Returns(value.Value);
        }
        this.PlayerService
            .SetupProperty(t => t.ActivePlayer, mockPlayer?.Object);
        return this;
    }
}
