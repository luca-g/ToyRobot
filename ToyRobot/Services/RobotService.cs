using Microsoft.Extensions.Logging;
using ToyRobot.Model;

namespace ToyRobot.Services;

public class RobotService
{
    private MapPosition? _mapPosition;
    private MapService _map;
    private readonly ILogger<RobotService> _logger;
    private readonly IRobotStepHistoryService _robotStepHistoryService;
    public RobotService(ILogger<RobotService> logger, MapService map, IRobotStepHistoryService robotStepHistoryService)
    {
        this._map = map;
        this._mapPosition = null;
        this._logger = logger;
        this._robotStepHistoryService = robotStepHistoryService;
    }
    public MapPosition? RobotPosition { get { return _mapPosition; } }
    public bool Execute(MapPosition mapPosition, string command, out string? result)
    {
        this._mapPosition = mapPosition;
        return this.Execute(command, out result);
    }
    public bool Execute(string command, out string? result)
    {
        result = null;
        var isValidCommand = true;
        var previousMapPosition = this._mapPosition;
        try
        {
            this._logger.LogTrace("Execute command: {command}", command);
            if (string.IsNullOrEmpty(command))
            {
                this._logger.LogTrace("Command is null or empty");
                isValidCommand = false;
                return false;
            }
            var commandParts = command.Split(new char[] { ' ', ',' });
            switch (commandParts[0])
            {
                case "PLACE":
                    if (commandParts.Count() != 4)
                    {
                        this._logger.LogTrace("PLACE command count error");
                        isValidCommand = false;
                        return false;
                    }
                    int x;
                    int y;
                    MapOrientationEnum mapOrientation;
                    if (!int.TryParse(commandParts[1], out x))
                    {
                        this._logger.LogTrace("PLACE invalid X value: {0}", commandParts[1]);
                        isValidCommand = false;
                        return false;
                    }
                    if (!int.TryParse(commandParts[2], out y))
                    {
                        this._logger.LogTrace("PLACE invalid Y value: {0}", commandParts[2]);
                        isValidCommand = false;
                        return false;
                    }
                    if (!Enum.TryParse(commandParts[3], out mapOrientation))
                    {
                        this._logger.LogTrace("PLACE invalid orientation value: {0}", commandParts[3]);
                        isValidCommand = false;
                        return false;
                    }
                    var mapPositon = new MapPosition(x, y, mapOrientation);
                    if (!this._map.IsInMap(mapPositon))
                    {
                        this._logger.LogTrace("PLACE position set outside the map");
                        isValidCommand = false;
                        return false;
                    }
                    this._mapPosition = mapPositon;
                    this._logger.LogTrace("PLACE command: robot at position {0},{1}", x, y);
                    break;
                case "RESIZE":
                    if (commandParts.Count() != 3)
                    {
                        this._logger.LogTrace("RESIZE command count error");
                        isValidCommand = false;
                        return false;
                    }
                    int w;
                    int h;
                    if (!int.TryParse(commandParts[1], out w))
                    {
                        this._logger.LogTrace("RESIZE invalid W value: {0}", commandParts[1]);
                        isValidCommand = false;
                        return false;
                    }
                    if (!int.TryParse(commandParts[2], out h))
                    {
                        this._logger.LogTrace("RESIZE invalid H value: {0}", commandParts[2]);
                        isValidCommand = false;
                        return false;
                    }
                    this._map.SetMapSize(w, h);
                    this._mapPosition = null;
                    this._robotStepHistoryService.AddResizeMapStep(w, h);
                    this._logger.LogTrace("RESIZE command: map resized to {0},{1}", w, h);
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
                case "SIZE":
                    result = string.Format("Map size {0},{1}", _map.Width, _map.Height);
                    this._logger.LogTrace("SIZE command result: {result}", result);
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
                    isValidCommand = false;
                    return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Execute error");
            throw;
        }
        finally
        {
            this._robotStepHistoryService.AddStep(previousMapPosition, this._mapPosition, command, isValidCommand, result);
        }
    }
}

