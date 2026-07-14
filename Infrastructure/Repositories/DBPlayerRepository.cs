using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace Infrastructure.Repositories
{
    public class DBPlayerRepository : IPlayerRepository
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly WorldRankDbContext _context;

        //constructor
        public DBPlayerRepository(WorldRankDbContext context)
        {
            _context = context;
        }

        public async Task AddPlayer(Player player, CancellationToken ct = default)
        {
            _context.Players.Add(player);
           await _context.SaveChangesAsync(ct);
            _logger.Info("Player {PlayerId} ({Name}) added with score {Score}", player.Id, player.Name, player.Score);
        }

        public async Task<IEnumerable<Player>> GetAllPlayers(CancellationToken ct = default)
        {
            return await _context.Players.ToListAsync(ct);
        }

        public async Task DeletePlayer(int playerId, CancellationToken ct = default)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == playerId, ct);

            if (player is null)
            {
                _logger.Warn("Delete skipped: player {PlayerId} not found", playerId);
                return;
            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync(ct);
            _logger.Info("Player {PlayerId} deleted", playerId);
        }

        public async Task<Player?> FindPlayer(int playerId, CancellationToken ct = default)
        {
            return await _context.Players.FirstOrDefaultAsync(p => p.Id == playerId, ct);
        }

        public async Task<IEnumerable<IGrouping<int, Player>>> GroupPlayersByScore(CancellationToken ct = default)
        {
            var players = await _context.Players.ToListAsync(ct);
            return players
                .GroupBy(player => player.Score)
                .OrderByDescending(group => group.Key);
        }
    }
}