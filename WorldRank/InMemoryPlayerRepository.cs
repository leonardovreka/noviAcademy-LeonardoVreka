namespace WorldRank;

public class InMemoryPlayerRepository : IPlayerRepository
{
    private readonly List<Player> _players = new();
    private int _nextId = 1;

    public Player AddPlayer(string name, int score)
    {
        var player = new Player(_nextId++, name);
        player.UpdateScore(score);
        _players.Add(player);
        return player;
    }

    public Player? FindPlayer(int playerId)
    {
        return _players.FirstOrDefault(p => p.Id == playerId);
    }

    public void DeletePlayer(int playerId)
    {
        var player = _players.FirstOrDefault(p => p.Id == playerId);
        if (player is not null)
            _players.Remove(player);
    }

    public IReadOnlyCollection<Player> GetAll() => _players.AsReadOnly();

    public IEnumerable<IGrouping<int, Player>> GroupPlayersByScore()
    {
        return _players
            .OrderByDescending(p => p.Score)
            .GroupBy(p => p.Score);
    }
}