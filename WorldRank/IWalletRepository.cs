namespace WorldRank;

public interface IWalletRepository
{
    void Add(IWallet wallet, int playerId);
    IReadOnlyCollection<IWallet> GetByPlayer(int playerId);
}