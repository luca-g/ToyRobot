using ToyRobot.Common.Model;

namespace ToyRobot.Common.Services;
public interface IPlayerService
{
    string Language => "en";
    IPlayer? ActivePlayer { get; set; }
    Task<IPlayer> LoadPlayer(Guid guid);
    Task<IPlayer> CreatePlayer();
}
