using ToyRobot.Common.Model;
using ToyRobot.Core.Configuration;

namespace ToyRobot.Common.Services;

public interface IMapService
{
    MapSettings MapSettings { get; }
    Task<IList<IMap>> LoadMaps(int playerId);
    Task<IMap> CreateMap(int playerId, int width, int heigth);
}