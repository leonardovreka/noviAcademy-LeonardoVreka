using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPlayerService
    {
        Task<Player> AddPlayer(string name, int score, CancellationToken ct = default);
        Task<Player?> GetPlayer(int playerId, CancellationToken ct = default);
        Task<IEnumerable<Player>> GetAllPlayers(CancellationToken ct = default);
    }
}