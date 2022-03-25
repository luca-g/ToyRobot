using ToyRobot.Common.Model;
using ToyRobot.Core.Configuration;

namespace ToyRobot.Common.Services;

public interface IMapService
{
    MapSettings MapSettings { get; }
    IMap? ActiveMap { get; set; }
    Task<IList<IMap>> LoadMaps(int playerId);
    Task<IMap> CreateMap(int width, int heigth);
}