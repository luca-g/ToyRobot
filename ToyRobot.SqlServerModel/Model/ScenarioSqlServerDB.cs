using ToyRobot.Common.Model;
using ToyRobot.Common.Services;
using ToyRobot.Core.Model;

namespace ToyRobot.SqlServerModel.Model;

public class ScenarioSqlServerDB : Scenario, IScenario
{
    private readonly IRobotServiceDB robotServiceDB;
    public ScenarioSqlServerDB(IRobotServiceDB robotServiceDB, IPlayer player, IMap? map, IRobot? robot) : base(player, map, robot)
    {
        this.robotServiceDB = robotServiceDB;
    }
    public new async Task<bool> SetMapPosition(IMapPosition? mapPosition)
    {
        if (this.Robot == null) return false;
        await robotServiceDB.SaveMapPosition(this.Robot, mapPosition, this.PlayerId);
        this.Robot.Position = mapPosition;
        return true;
    }
}
