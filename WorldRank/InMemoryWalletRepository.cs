namespace WorldRank;

public class InMemoryWalletRepository : IWalletRepository
{
    private readonly IPlayerRepository _playerRepo;

    public InMemoryWalletRepository(IPlayerRepository playerRepo)
    {
        _playerRepo = playerRepo;
    }

    public void Add(IWallet wallet, int playerId)
    {
        var player = _playerRepo.FindPlayer(playerId);
        if (player is null)
            throw new InvalidOperationException($"No player with ID {playerId}.");

        player.AddWallet(wallet);   // single source of truth + one-per-currency check happens here
    }

    public IReadOnlyCollection<IWallet> GetByPlayer(int playerId)
    {
        var player = _playerRepo.FindPlayer(playerId);
        return player is null
            ? Array.Empty<Wallet>()
            : player.Wallets;
    }
}