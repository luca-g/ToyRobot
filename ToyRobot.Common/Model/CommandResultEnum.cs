using System.ComponentModel;

namespace ToyRobot.Common.Model;

public enum CommandResultEnum
{
    NotSet,
    Ok,
    UnexpectedError,
    ActiveMapNull,
    ActiveRobotNull,
    MapCreateError,
    MapCreatedIdWH,
    CreateRobotFailed,
    RobotCreatedId,
    RobotPositionNull,
    RobotCannotMoveOutsideMap
}
