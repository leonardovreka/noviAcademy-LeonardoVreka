using Domain.Entities;

namespace Application.Interfaces
{
	public interface IPlayerRepository
	{
		void AddPlayer(Player player);

		IEnumerable<Player> GetAllPlayers();

		bool DeletePlayer(int playerId);

		Player? FindPlayer(int playerId);

		IEnumerable<IGrouping<int, Player>> GroupPlayersByScore();
	}
}