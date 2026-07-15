using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;

namespace Infrastructure.Repositories
{
    public class InMemoryWalletRepository : IWalletRepository
    {
        private readonly List<Wallet> _wallets = new List<Wallet>();

        public void Add(Wallet wallet)
        {
            var exists = _wallets.Any(item => item.PlayerId == wallet.PlayerId && item.Currency == wallet.Currency);

            if (exists)
            {
                throw new DuplicateWalletException(wallet.PlayerId, wallet.Currency);
            }

            _wallets.Add(wallet);
        }

        public List<Wallet> GetAllWalletsByPlayerId(int playerId)
        {
            return _wallets.Where(item => item.PlayerId == playerId).ToList();
        }

        public void UpdateBalance(int playerId, Currency currency, decimal newBalance)
        {
            GetWallet(playerId, currency).SetBalance(newBalance);
        }

        public Wallet Deposit(int playerId, Currency currency, decimal amount)
        {
            var wallet = GetWallet(playerId, currency);
            wallet.Deposit(amount);
            return wallet;
        }

        public Wallet Withdraw(int playerId, Currency currency, decimal amount)
        {
            var wallet = GetWallet(playerId, currency);
            wallet.Withdraw(amount);
            return wallet;
        }

        public void Block(int playerId, Currency currency)
        {
            GetWallet(playerId, currency).Block();
        }

        public void Unblock(int playerId, Currency currency)
        {
            GetWallet(playerId, currency).Unblock();
        }

        public Wallet GetWallet(int playerId, Currency currency)
        {
            var wallet = _wallets.SingleOrDefault(item => item.PlayerId == playerId && item.Currency == currency);

            if (wallet is null)
            {
                throw new WalletNotFoundException(playerId, currency);
            }

            return wallet;
        }
    }
}