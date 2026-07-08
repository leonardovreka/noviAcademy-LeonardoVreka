namespace WorldRank;

public interface IPlayerRepository
{
    Player AddPlayer(string name, int score);
    Player? FindPlayer(int playerId);
    void DeletePlayer(int playerId);
    IReadOnlyCollection<Player> GetAll();
    IEnumerable<IGrouping<int, Player>> GroupPlayersByScore();
}