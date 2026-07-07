namespace WorldRank;

public interface IWalletRepository
{
    void Add(Wallet wallet, int playerId);
    IReadOnlyCollection<Wallet> GetByPlayer(int playerId);
}