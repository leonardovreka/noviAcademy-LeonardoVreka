using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Context;
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

        public void AddPlayer(Player player)
        {
            _context.Players.Add(player);
            _context.SaveChanges();
            _logger.Info("Player {PlayerId} ({Name}) added with score {Score}", player.Id, player.Name, player.Score);
        }

        public IEnumerable<Player> GetAllPlayers()
        {
            return _context.Players.ToList();
        }

        public void DeletePlayer(int playerId)
        {
            var player = _context.Players.FirstOrDefault(p => p.Id == playerId);

            if (player is null)
            {
                _logger.Warn("Delete skipped: player {PlayerId} not found", playerId);
                return;
            }

            _context.Players.Remove(player);
            _context.SaveChanges();
            _logger.Info("Player {PlayerId} deleted", playerId);
        }

        public Player? FindPlayer(int playerId)
        {
            return _context.Players.FirstOrDefault(p => p.Id == playerId);
        }

        public IEnumerable<IGrouping<int, Player>> GroupPlayersByScore()
        {
            return _context.Players
                .AsEnumerable()
                .GroupBy(player => player.Score)
                .OrderByDescending(group => group.Key);
        }
    }
}