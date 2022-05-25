using ToyRobot.Common.Model;
using ToyRobot.Core.Configuration;

namespace ToyRobot.Common.Services;

public interface IMapService
{
    MapSettings MapSettings { get; }
    IMap? LoadMap(int mapId);
    Task<IList<IMap>> LoadMaps(int playerId);
    Task<IMap> CreateMap(int playerId, int width, int heigth);
}