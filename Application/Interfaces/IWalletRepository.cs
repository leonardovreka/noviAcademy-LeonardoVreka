using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces
{
    public interface IWalletRepository
    {
        Task Add(Wallet wallet, CancellationToken ct = default);
        Task<List<Wallet>> GetAllWalletsByPlayerId(int playerId, CancellationToken ct = default);
        Task<Wallet> GetWallet(int playerId, Currency currency, CancellationToken ct = default);
        Task UpdateBalance(int playerId, Currency currency, decimal newBalance, CancellationToken ct = default);
        Task Deposit(int playerId, Currency currency, decimal amount, CancellationToken ct = default);
        Task Withdraw(int playerId, Currency currency, decimal amount, CancellationToken ct = default);
        Task Block(int playerId, Currency currency, CancellationToken ct = default);
        Task Unblock(int playerId, Currency currency, CancellationToken ct = default);
    }
}