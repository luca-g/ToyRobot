using Microsoft.Extensions.Logging;
using ToyRobot.Model;

namespace ToyRobot.Services;

public class RobotService
{
    private MapPosition? _mapPosition;
    private MapService _map;
    private readonly ILogger<RobotService> _logger;
    public RobotService(ILogger<RobotService> logger, MapService map)
    {
        this._map = map;
        this._mapPosition = null;
        this._logger = logger;
    }
    public bool Execute(string command, out string? result)
    {
        try
        {
            this._logger.LogTrace("Execute command: {command}", command);
            result = null;
            if (string.IsNullOrEmpty(command))
            {
                this._logger.LogTrace("Command is null or empty");
                return false;
            }
            var commandParts = command.Split(new char[] { ' ', ',' });
            switch (commandParts[0])
            {
                case "PLACE":
                    if (commandParts.Count() != 4)
                    {
                        this._logger.LogTrace("PLACE command count error");
                        return false;
                    }
                    int x;
                    int y;
                    MapOrientationEnum mapOrientation;
                    if (!int.TryParse(commandParts[1], out x))
                    {
                        this._logger.LogTrace("PLACE invalid X value: {0}", commandParts[1]);
                        return false;
                    }
                    if (!int.TryParse(commandParts[2], out y))
                    {
                        this._logger.LogTrace("PLACE invalid Y value: {0}", commandParts[2]);
                        return false;
                    }
                    if (!Enum.TryParse(commandParts[3], out mapOrientation))
                    {
                        this._logger.LogTrace("PLACE invalid orientation value: {0}", commandParts[3]);
                        return false;
                    }
                    var mapPositon = new MapPosition(x, y, mapOrientation);
                    if (!this._map.IsInMap(mapPositon))
                    {
                        this._logger.LogTrace("PLACE position set outside the map");
                        this._mapPosition = null;
                        return true;
                    }
                    this._mapPosition = mapPositon;
                    this._logger.LogTrace("PLACE command: robot at position {0},{1}", x, y);
                    break;
                case "MOVE":
                    if (this._mapPosition == null)
                    {
                        this._logger.LogTrace("MOVE command: The robot is not in the map");
                        return true;
                    }
                    var newPosition = this._mapPosition.Move();
                    if (!this._map.IsInMap(newPosition))
                    {
                        this._logger.LogTrace("MOVE command: The robot cannot move outside the map");
                        return true;
                    }
                    this._mapPosition = newPosition;
                    this._logger.LogTrace("PLACE command: robot moved to position {0},{1}", this._mapPosition.X, this._mapPosition.Y);
                    break;
                case "REPORT":
                    if (this._mapPosition == null)
                    {
                        result = "Robot out of map";
                        this._logger.LogTrace("REPORT command result: {result}", result);
                        return true;
                    }
                    result = this._mapPosition.ToString();
                    this._logger.LogTrace("REPORT command result: {result}", result);
                    break;
                case "LEFT":
                    if (this._mapPosition == null)
                    {
                        this._logger.LogTrace("LEFT command: The robot is not in the map");
                        return true;
                    }
                    this._mapPosition.Left();
                    this._logger.LogTrace("LEFT command: new orientation {0}", this._mapPosition.Orientation);
                    break;
                case "RIGHT":
                    if (this._mapPosition == null)
                    {
                        this._logger.LogTrace("RIGHT command: The robot is not in the map");
                        return true;
                    }
                    this._mapPosition.Right();
                    this._logger.LogTrace("RIGHT command: new orientation {0}", this._mapPosition.Orientation);
                    break;
                default:
                    this._logger.LogTrace("Invalid command: {0}", commandParts[0]);
                    return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Execute error");
            throw;
        }
    }
}

