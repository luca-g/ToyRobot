namespace ToyRobot.Common.Model;

public interface IPlayer
{
    int PlayerId { get; }
    Guid PlayerGuid { get; }
}
