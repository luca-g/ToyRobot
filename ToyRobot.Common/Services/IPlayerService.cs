using ToyRobot.Common.Model;

namespace ToyRobot.Common.Services;
public interface IPlayerService
{
    string Language => "en";
    Task<IPlayer> LoadPlayer(Guid guid);
    Task<IPlayer> CreatePlayer();
}
