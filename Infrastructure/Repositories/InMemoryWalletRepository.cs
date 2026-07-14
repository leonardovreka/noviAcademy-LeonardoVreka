using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using NLog;

namespace Infrastructure.Repositories
{
    public class InMemoryWalletRepository : IWalletRepository
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly List<Wallet> _wallets = new List<Wallet>();

        public Task Add(Wallet wallet, CancellationToken ct = default)
        {
            var exists = _wallets.Any(item => item.PlayerId == wallet.PlayerId && item.Currency == wallet.Currency);

            if (exists)
                throw new DuplicateWalletException(wallet.PlayerId, wallet.Currency);

            _wallets.Add(wallet);
            _logger.Info("Wallet created for player {PlayerId} in {Currency} with balance {Balance}", wallet.PlayerId, wallet.Currency, wallet.Balance);
            return Task.CompletedTask;
        }

        public Task<List<Wallet>> GetAllWalletsByPlayerId(int playerId, CancellationToken ct = default)
        {
            return Task.FromResult(_wallets.Where(item => item.PlayerId == playerId).ToList());
        }

        public Task<Wallet> GetWallet(int playerId, Currency currency, CancellationToken ct = default)
        {
            var wallet = _wallets.SingleOrDefault(item => item.PlayerId == playerId && item.Currency == currency);

            if (wallet is null)
                throw new WalletNotFoundException(playerId, currency);

            return Task.FromResult(wallet);
        }

        public Task UpdateBalance(int playerId, Currency currency, decimal newBalance, CancellationToken ct = default)
        {
            GetWallet(playerId, currency).GetAwaiter().GetResult().SetBalance(newBalance);
            _logger.Info("Player {PlayerId} {Currency} wallet balance set to {Balance}", playerId, currency, newBalance);
            return Task.CompletedTask;
        }

        public Task Deposit(int playerId, Currency currency, decimal amount, CancellationToken ct = default)
        {
            var wallet = GetWallet(playerId, currency).GetAwaiter().GetResult();
            wallet.Deposit(amount);
            _logger.Info("Deposited {Amount} to player {PlayerId} {Currency} wallet", amount, playerId, currency);
            return Task.CompletedTask;
        }

        public Task Withdraw(int playerId, Currency currency, decimal amount, CancellationToken ct = default)
        {
            var wallet = GetWallet(playerId, currency).GetAwaiter().GetResult();
            wallet.Withdraw(amount);
            _logger.Info("Withdrew {Amount} from player {PlayerId} {Currency} wallet", amount, playerId, currency);
            return Task.CompletedTask;
        }

        public Task Block(int playerId, Currency currency, CancellationToken ct = default)
        {
            GetWallet(playerId, currency).GetAwaiter().GetResult().Block();
            _logger.Info("Player {PlayerId} {Currency} wallet blocked", playerId, currency);
            return Task.CompletedTask;
        }

        public Task Unblock(int playerId, Currency currency, CancellationToken ct = default)
        {
            GetWallet(playerId, currency).GetAwaiter().GetResult().Unblock();
            _logger.Info("Player {PlayerId} {Currency} wallet unblocked", playerId, currency);
            return Task.CompletedTask;
        }
    }
}