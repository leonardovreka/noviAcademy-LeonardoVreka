namespace WorldRank;

public class InMemoryPlayerRepository : IPlayerRepository
{
    private readonly List<Player> _players = new();
    private int _nextId = 1;

    public Player AddPlayer(Player player)
    {
        var newPlayer = new Player(_nextId++, player.Name);
        newPlayer.UpdateScore(player.Score);
        _players.Add(newPlayer);
        return newPlayer;
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