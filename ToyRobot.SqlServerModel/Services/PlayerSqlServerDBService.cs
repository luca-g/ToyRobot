using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using ToyRobot.Common.Model;
using ToyRobot.Common.Services;
using ToyRobot.SqlServerModel.DB;

namespace ToyRobot.SqlServerModel.Services;

public class PlayerSqlServerDBService : IPlayerService
{
    public IPlayer? ActivePlayer { get; set; }

    private readonly ToyRobotDbContext _toyRobotDbContext;
    private readonly ILogger<PlayerSqlServerDBService> _logger;

    public PlayerSqlServerDBService(ILogger<PlayerSqlServerDBService> logger, ToyRobotDbContext toyRobotDbContext)
    {
        this._toyRobotDbContext = toyRobotDbContext;
        this._logger = logger;
    }

    public async Task<IPlayer> CreatePlayer()
    {
        try
        {
            _logger.LogTrace("CreatePlayer: creating player");
            var player = new Player
            {
                CreationDate = DateTime.UtcNow,
                Identifier = Guid.NewGuid()
            };
            _toyRobotDbContext.Player.Add(player);
            await _toyRobotDbContext.SaveChangesAsync();            
            this.ActivePlayer = player;
            _logger.LogTrace("CreatePlayer: player created - player {PlayerId}", this.ActivePlayer.PlayerId);
            return this.ActivePlayer;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating player");
            throw;
        }
    }

    public async Task<IPlayer> LoadPlayer(Guid guid)
    {
        try {
            _logger.LogTrace("Loading player {guid}", guid);
            var player = await _toyRobotDbContext.Player.SingleOrDefaultAsync(t=>t.PlayerGuid == guid);
            if (player == null)
            {
                throw new Exception($"Player not found {guid}");
            }
            _logger.LogTrace("Loaded player {guid}",guid);
            this.ActivePlayer = player;
            return player;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading player");
            throw;
        }
    }

}
