using ToyRobot.Common.Model;
using ToyRobot.Common.Services;
using ToyRobot.Core.Model;

namespace ToyRobot.SqlServerModel.Model;

public class ScenarioSqlServerDB : Scenario, IScenario
{
    private readonly IRobotServiceDB robotServiceDB;
    public ScenarioSqlServerDB(IPlayer player, IMap? map, IRobot? robot) : base(player, map, robot)
    {
        var services = ToyRobotServices.Instance;
        if (services == null)
        {
            throw new NullReferenceException("ToyRobotServices cannot be null");
        }
        if (services.GetService<IRobotService>() is not IRobotServiceDB robotService)
        {
            throw new NullReferenceException("IRobotServiceDB service cannot be null");
        }
        this.robotServiceDB = robotService;
    }
    public new async Task<bool> SetMapPosition(IMapPosition? mapPosition)
    {
        if (this.Robot == null) return false;
        await robotServiceDB.SaveMapPosition(this.Robot, mapPosition, this.PlayerId);
        this.Robot.Position = mapPosition;
        return true;
    }
}
