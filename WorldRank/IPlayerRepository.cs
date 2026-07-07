namespace WorldRank;

public interface IPlayerRepository
{
    Player AddPlayer(Player player);
    Player? FindPlayer(int playerId);
    void DeletePlayer(int playerId);
    IReadOnlyCollection<Player> GetAll();
    IEnumerable<IGrouping<int, Player>> GroupPlayersByScore();
}