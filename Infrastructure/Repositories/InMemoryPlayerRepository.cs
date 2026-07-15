using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Repositories
{
    public class InMemoryPlayerRepository : IPlayerRepository
    {
        private readonly IMemoryCache _cache;

        private List<Player> _players;

        public InMemoryPlayerRepository(IMemoryCache cache)
        {
            _players = new List<Player>();
            _cache = cache;
        }

        public void AddPlayer(Player player)
        {
            _players.Add(player);
        }

        public IEnumerable<Player> GetAllPlayers()
        {
            if (_cache.TryGetValue("AllPlayersKey", out IReadOnlyList<Player>? cached) && cached is not null)
            {
                return cached;
            }

            var players = _players.ToList();

            _cache.Set("AllPlayersKey", players, TimeSpan.FromSeconds(60));

            return players;
        }

        public bool DeletePlayer(int playerId)
        {
            var player = _players.Where(item => item.Id == playerId).FirstOrDefault();

            if (player is null)
            {
                return false;
            }

            _players.Remove(player);
            return true;
        }

        public Player? FindPlayer(int playerId)
        {
            return _players.Where(item => item.Id == playerId).FirstOrDefault();
        }

        public IEnumerable<IGrouping<int, Player>> GroupPlayersByScore()
        {
            return _players
                .GroupBy(player => player.Score)
                .OrderByDescending(group => group.Key);
        }
    }
}