using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Repositories
{
    public class DBWalletRepository : IWalletRepository
    {
        private readonly WorldRankDbContext _context;

        public DBWalletRepository(WorldRankDbContext context)
        {
            _context = context;
        }

        public void Add(Wallet wallet)
        {
            var exists = _context.Wallets.Any(w => w.PlayerId == wallet.PlayerId && w.Currency == wallet.Currency);

            if (exists)
            {
                throw new DuplicateWalletException(wallet.PlayerId, wallet.Currency);
            }

            _context.Wallets.Add(wallet);
            _context.SaveChanges();
        }

        public List<Wallet> GetAllWalletsByPlayerId(int playerId)
        {
            return _context.Wallets
                .Where(w => w.PlayerId == playerId)
                .ToList();
        }

        public Wallet GetWallet(int playerId, Currency currency)
        {
            var wallet = _context.Wallets
                .SingleOrDefault(w => w.PlayerId == playerId && w.Currency == currency);

            if (wallet is null)
            {
                throw new WalletNotFoundException(playerId, currency);
            }

            return wallet;
        }

        public void UpdateBalance(int playerId, Currency currency, decimal newBalance)
        {
            var wallet = GetWallet(playerId, currency);
            wallet.SetBalance(newBalance);
            _context.SaveChanges();
        }

        // returns the updated wallet so the caller can log the resulting balance
        public Wallet Deposit(int playerId, Currency currency, decimal amount)
        {
            var wallet = GetWallet(playerId, currency);
            wallet.Deposit(amount);
            _context.SaveChanges();
            return wallet;
        }

        public Wallet Withdraw(int playerId, Currency currency, decimal amount)
        {
            var wallet = GetWallet(playerId, currency);
            wallet.Withdraw(amount);
            _context.SaveChanges();
            return wallet;
        }

        public void Block(int playerId, Currency currency)
        {
            var wallet = GetWallet(playerId, currency);
            wallet.Block();
            _context.SaveChanges();
        }

        public void Unblock(int playerId, Currency currency)
        {
            var wallet = GetWallet(playerId, currency);
            wallet.Unblock();
            _context.SaveChanges();
        }
    }
}