using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using NLog;

namespace Infrastructure.Repositories
{
    public class InMemoryPlayerRepository : IPlayerRepository
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IMemoryCache _cache;
        private List<Player> _players;

        public InMemoryPlayerRepository(IMemoryCache cache)
        {
            _players = new List<Player>();
            _cache = cache;
        }

        public Task AddPlayer(Player player, CancellationToken ct = default)
        {
            _players.Add(player);
            _logger.Info("Player {PlayerId} ({Name}) added with score {Score}", player.Id, player.Name, player.Score);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Player>> GetAllPlayers(CancellationToken ct = default)
        {
            if (_cache.TryGetValue("AllPlayersKey", out IReadOnlyList<Player>? cached) && cached is not null)
                return Task.FromResult<IEnumerable<Player>>(cached);

            var players = _players.ToList();
            _cache.Set("AllPlayersKey", players, TimeSpan.FromSeconds(60));
            return Task.FromResult<IEnumerable<Player>>(players);
        }

        public Task DeletePlayer(int playerId, CancellationToken ct = default)
        {
            var player = _players.FirstOrDefault(item => item.Id == playerId);

            if (player is null)
            {
                _logger.Warn("Delete skipped: player {PlayerId} not found", playerId);
                return Task.CompletedTask;
            }

            _players.Remove(player);
            _logger.Info("Player {PlayerId} deleted", playerId);
            return Task.CompletedTask;
        }

        public Task<Player?> FindPlayer(int playerId, CancellationToken ct = default)
        {
            return Task.FromResult(_players.FirstOrDefault(item => item.Id == playerId));
        }

        public Task<IEnumerable<IGrouping<int, Player>>> GroupPlayersByScore(CancellationToken ct = default)
        {
            var result = _players
                .GroupBy(player => player.Score)
                .OrderByDescending(group => group.Key);

            return Task.FromResult<IEnumerable<IGrouping<int, Player>>>(result);
        }
    }
}