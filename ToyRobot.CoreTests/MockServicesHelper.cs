using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics;
using System.Threading.Tasks;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;

namespace ToyRobot.Core.Tests;
public class MockServicesHelper<T>
{
    public Mock<IScenario> Scenario = new();
    public Mock<ILogger<T>> Logger = new();
    public Mock<IMapService> MapService = new();
    public Mock<IPlayerService> PlayerService = new();
    public Mock<IRobotService> RobotService = new();
    public Mock<IRobotStepHistoryService> RobotStepHistoryService = new();
    public IApplicationMessagesService ApplicationMessageService = new DefaultApplicationMessagesService();
    public Mock<ICommandCenterService> CommandCenterService = new();

    Mock<IMap>? mockMap = null;
    Mock<IRobot>? mockRobot = null;

    public int SetPositionCalled = 0;
    public int SetActiveMapCalled = 0;

    public MockServicesHelper<T> CreateMapSetup()
    {
        if (mockMap == null)
        {
            mockMap = new();
        }
        mockMap.SetupGet(t => t.MapId).Returns(1);
        Scenario.SetupGet(t=>t.PlayerId).Returns(1);
        Scenario
            .Setup(t => t.SetActiveMap(It.IsAny<IMap>()))
            .Callback(() => SetActiveMapCalled++);

        this.MapService
            .Setup(t => t.CreateMap(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns(Task.FromResult(mockMap.Object));
        return this;
    }
    public MockServicesHelper<T> CreateRobotSetup()
    {
        if (mockRobot == null)
        {
            mockRobot = new();
        }
        mockRobot.SetupGet(t => t.RobotId).Returns(1);

        this.RobotService
            .Setup(t => t.CreateRobot(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(Task.FromResult(mockRobot.Object));
        return this;
    }

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

    public MockServicesHelper<T> SetActiveRobotPosition(int? x, int? y, MapOrientationEnum? orientationEnum)
    {
        Mock<IMapPosition>? mockPosition = null;
        if (x.HasValue && y.HasValue && orientationEnum.HasValue)
        {
            mockPosition = new();
            mockPosition.SetupGet(t => t.X).Returns(x.Value);
            mockPosition.SetupGet(t => t.Y).Returns(y.Value);
            mockPosition.SetupGet(t => t.Orientation).Returns(orientationEnum.Value);

            var mockPositionLRMove = new Mock<IMapPosition>();
            mockPositionLRMove.SetupGet(t => t.X).Returns(x.Value);
            mockPositionLRMove.SetupGet(t => t.Y).Returns(y.Value);
            mockPositionLRMove.SetupGet(t => t.Orientation).Returns(orientationEnum.Value);

            mockPosition.Setup(t => t.Left()).Returns(mockPositionLRMove.Object);
            mockPosition.Setup(t => t.Right()).Returns(mockPositionLRMove.Object);
            mockPosition.Setup(t => t.Move()).Returns(mockPositionLRMove.Object);
        }

        this.Scenario
            .SetupGet(t => t.IsRobotSet).Returns(true);
        this.Scenario
            .SetupGet(t => t.IsRobotDeployed).Returns(mockPosition!=null);
        this.Scenario
            .SetupGet(t => t.RobotId).Returns(1);
        this.Scenario
            .SetupGet(t => t.RobotPosition).Returns(mockPosition?.Object);

        this.Scenario
            .Setup(t => t.SetMapPosition(It.IsAny<IMapPosition>()))
            .Callback(() => SetPositionCalled++);
        return this;
    }

    public MockServicesHelper<T> ActiveMapSetupProperty(int? value, bool isInMapResult = true)
    {        
        this.Scenario
            .SetupGet(t => t.MapId).Returns(value);
        this.Scenario
            .SetupGet(t => t.IsMapSet).Returns(true);
        this.Scenario
            .Setup(t => t.IsInMap(It.IsAny<IMapPoint>())).Returns(isInMapResult);
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
