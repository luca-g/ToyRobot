using ToyRobot.Common.Model;

namespace ToyRobot.Common.Services;

public interface IFactoryService
{
    Task<IScenario> CreateScenario();
    Task<IScenario> CreateScenario(Guid? playerId, int? mapId, int? robotId);
}
