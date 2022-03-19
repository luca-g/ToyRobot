using ToyRobot.Common.Model;

namespace ToyRobot.Common.Services;
public interface IPlayerService
{
    int? PlayerId { get; }
    Guid? PlayerIdentifier { get; }
    Task CreatePlayer();
    Task<List<IMap>?> AvailableMaps();
    Task SetActiveMap(IMap map);
    Task<IMap> CreateMap(int width, int heigth);
}
