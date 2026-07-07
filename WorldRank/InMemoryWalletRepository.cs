namespace WorldRank;

public class InMemoryWalletRepository : IWalletRepository
{
    private readonly Dictionary<int, List<Wallet>> _walletsByPlayer = new();

    public void Add(Wallet wallet, int playerId)
    {
        if (!_walletsByPlayer.ContainsKey(playerId))
            _walletsByPlayer[playerId] = new List<Wallet>();

        _walletsByPlayer[playerId].Add(wallet);
    }

    public IReadOnlyCollection<Wallet> GetByPlayer(int playerId)
    {
        return _walletsByPlayer.TryGetValue(playerId, out var wallets)
            ? wallets.AsReadOnly()
            : Array.Empty<Wallet>();
    }
}