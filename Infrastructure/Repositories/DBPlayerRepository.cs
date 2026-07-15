using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Repositories
{
    public class DBPlayerRepository : IPlayerRepository
    {

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
        }

        public IEnumerable<Player> GetAllPlayers()
        {
            return _context.Players.ToList();
        }

        public bool DeletePlayer(int playerId)
        {
            var player = _context.Players.FirstOrDefault(p => p.Id == playerId);

            if (player is null)
            {
                return false;
            }

            _context.Players.Remove(player);
            _context.SaveChanges();
            return true;
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