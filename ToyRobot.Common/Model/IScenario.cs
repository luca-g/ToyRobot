
namespace ToyRobot.Common.Model
{
    public interface IScenario
    {
        bool IsMapSet { get; }
        bool IsRobotDeployed { get; }
        bool IsRobotSet { get; }
        int? MapId { get; }
        int PlayerId { get; }
        int? RobotId { get; }
        IMapPosition? RobotPosition { get; }

        bool IsInMap(IMapPoint mapPoint);
        string MapSize();
        Task<bool> SetActiveMap(IMap map);
        Task<bool> SetActiveRobot(IRobot? robot);
        Task<bool> SetMapPosition(IMapPosition mapPosition);
    }
}