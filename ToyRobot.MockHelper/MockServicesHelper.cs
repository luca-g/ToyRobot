using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;

namespace ToyRobot.MockHelper;
public class MockServicesHelper<T>
{
    public Mock<ILogger<T>> Logger = new();
    public Mock<IMapService> MapService = new();
    public Mock<IPlayerService> PlayerService = new();
    public Mock<IRobotService> RobotService = new();
    public Mock<IRobotStepHistoryService> RobotStepHistoryService = new();

    Mock<IMap>? mockMap = null;
    Mock<IRobot>? mockRobot = null;
    Mock<IPlayer>? mockPlayer = null;

    public MockServicesHelper<T> CreateMapSetup()
    {
        if (mockMap == null)
        {
            mockMap = new();
        }
        mockMap.SetupGet(t => t.MapId).Returns(1);

        this.MapService
            .Setup(t => t.CreateMap(It.IsAny<int>(), It.IsAny<int>()))
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
        if (value.HasValue)
        {
            Debug.Assert(mockMap != null);
            Debug.Assert(mockPlayer != null);

            mockRobot = new();
            mockRobot.SetupGet(t => t.RobotId).Returns(value.Value);
            mockRobot.SetupGet(t => t.Map).Returns(mockMap.Object);
            mockRobot.SetupGet(t => t.Player).Returns(mockPlayer.Object);
            mockRobot.SetupGet(t => t.Position);
        }
        else
            mockRobot = null;

        this.RobotService
            .SetupProperty(t => t.ActiveRobot, mockRobot?.Object);
        return this;
    }
    public MockServicesHelper<T> SetActiveRobotPosition(int? x, int? y, MapOrientationEnum? orientationEnum)
    {
        Debug.Assert(mockRobot != null);
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
        mockRobot.SetupGet(t => t.Position).Returns(mockPosition?.Object);
        return this;
    }

    public MockServicesHelper<T> ActiveMapSetupProperty(int? value)
    {        
        if (value.HasValue)
        {
            if (mockMap == null)
            {
                mockMap = new Mock<IMap>();
            }
            mockMap.SetupGet(t => t.MapId).Returns(value.Value);
            mockMap.SetupGet(t => t.Width).Returns(5);
            mockMap.SetupGet(t => t.Height).Returns(5);
            mockMap.Setup(t => t.Size()).Returns("Width 5, Height 5");
            mockMap.Setup(t => t.IsInMap(It.IsAny<IMapPoint>())).Returns(true);
        }        
        this.MapService
            .SetupProperty(t => t.ActiveMap, mockMap?.Object);
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
