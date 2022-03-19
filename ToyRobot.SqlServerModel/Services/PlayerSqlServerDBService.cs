using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;
using ToyRobot.SqlServerModel.DB;

namespace ToyRobot.SqlServerModel.Services;

internal class PlayerSqlServerDBService : IPlayerService
{
    public int? PlayerId => CurrentPlayer?.PlayerId;
    public Guid? PlayerIdentifier => CurrentPlayer?.Identifier;
    public Player? CurrentPlayer {  get; private set; }
    public IMap? CurrentMap { get; private set; }

    List<IMap>? Maps;
    private readonly ToyRobotDbContext _toyRobotDbContext;
    private readonly ILogger<PlayerSqlServerDBService> _logger;

    public PlayerSqlServerDBService(ILogger<PlayerSqlServerDBService> logger, ToyRobotDbContext toyRobotDbContext)
    {
        this._toyRobotDbContext = toyRobotDbContext;
        this._logger = logger;
    }

    public async Task<List<IMap>?> AvailableMaps()
    {
        Debug.Assert(PlayerId != null);
        try
        {
            _logger.LogTrace("AvailableMaps: Loading maps for player {0}", PlayerId);
            this.Maps = await _toyRobotDbContext
                .Map
                .Where(m => m.CreatedByPlayerId == PlayerId)
                .OrderBy(t => t.Width)
                .ThenBy(t => t.Height)
                .Select(t => t as IMap)
                .ToListAsync();
            _logger.LogTrace("AvailableMaps: Loaded maps for player {0}, maps found: {1}", PlayerId, this.Maps.Count);
            return this.Maps;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error loading maps");
            throw;
        }
    }

    public async Task<IMap> CreateMap(int width, int heigth)
    {
        Debug.Assert(CurrentPlayer!=null);
        try
        {
            _logger.LogTrace("CreateMap looking for existing map of same size - player {0}", PlayerId);
            var map = await _toyRobotDbContext
                .Map
                .FirstOrDefaultAsync(m => m.CreatedByPlayerId == PlayerId && m.Width == width && m.Height == heigth);
            if (map != null)
            {
                _logger.LogTrace("CreateMap found map of same size - player {0}, map {1}", PlayerId, map.MapId);
                return map;
            }
            map = new Map
            {
                Width = width,
                Height = heigth,
                CreatedByPlayerId = PlayerId,
                CreationDate = DateTime.UtcNow,
                MaxRobots = RobotSqlServerDBService.DefaultMaxRobots
            };
            CurrentPlayer.Map.Add(map);
            await _toyRobotDbContext.SaveChangesAsync();
            Debug.Assert(map.MapId > 0);
            _logger.LogTrace("CreateMap: map created - player {0}, map {1}", PlayerId, map.MapId);
            return map;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error creating map");
            throw;
        }
    }

    public async Task CreatePlayer()
    {
        try
        {
            _logger.LogTrace("CreatePlayer: creating player");
            CurrentPlayer = new Player
            {
                CreationDate = DateTime.UtcNow,
                Identifier = Guid.NewGuid()
            };
            _toyRobotDbContext.Player.Add(CurrentPlayer);
            await _toyRobotDbContext.SaveChangesAsync();
            _logger.LogTrace("CreatePlayer: player created - player {0}", PlayerId);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating player");
            throw;
        }
    }

    public Task SetActiveMap(IMap map)
    {
        this.CurrentMap = map;
        _logger.LogTrace("SetActiveMap: map activated {0}", map.MapId);
        return Task.CompletedTask;
    }
}
