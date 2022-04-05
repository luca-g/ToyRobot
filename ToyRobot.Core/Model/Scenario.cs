using ToyRobot.Common.Model;

namespace ToyRobot.Core.Model;

public class Scenario : IScenario
{
    protected IPlayer Player { get; set; }
    protected IMap? Map { get; set; }
    protected IRobot? Robot { get; set; }
    public bool IsMapSet => Map != null;
    public bool IsRobotSet => Robot != null;
    public bool IsRobotDeployed => Robot?.Position != null;
    public int PlayerId => this.Player.PlayerId;
    public int? MapId => this.Map?.MapId;
    public int? RobotId => this.Robot?.RobotId;
    public IMapPosition? RobotPosition => Robot?.Position;
    public Scenario(IPlayer player, IMap? map, IRobot? robot)
    {
        this.Player = player;
        this.Map = map;
        this.Robot = robot;
    }
    public bool IsInMap(IMapPoint mapPoint)
    {
        if (this.Map == null) return false;
        return this.Map.IsInMap(mapPoint);
    }
    public Task<bool> SetMapPosition(IMapPosition mapPosition)
    {
        if (this.Robot == null) return Task.FromResult(false);
        this.Robot.Position = mapPosition;
        return Task.FromResult(true);
    }
    public string MapSize()
    {
        if (this.Map == null) return string.Empty;
        return this.Map.Size();
    }
    public Task<bool> SetActiveMap(IMap map)
    {
        this.Map = map;
        this.Robot = null;
        return Task.FromResult(true);
    }
    public Task<bool> SetActiveRobot(IRobot? robot)
    {
        if (this.Map == null) return Task.FromResult(false);
        this.Robot = robot;
        return Task.FromResult(true);
    }
}
