using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces
{
    public interface IWalletService
    {
        Task<Wallet> CreateWallet(int playerId, Currency currency, decimal initialBalance, CancellationToken ct = default);
        Task<Wallet> GetWallet(int playerId, Currency currency, CancellationToken ct = default);
        Task Deposit(int playerId, Currency currency, decimal amount, CancellationToken ct = default);
    }
}