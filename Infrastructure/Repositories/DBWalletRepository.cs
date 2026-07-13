using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Infrastructure.Persistence.Context;
using NLog;

namespace Infrastructure.Repositories
{
    public class DBWalletRepository : IWalletRepository
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly WorldRankDbContext _context;

        //Constructor
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
            _logger.Info("Wallet created for player {PlayerId} in {Currency} with balance {Balance}", wallet.PlayerId, wallet.Currency, wallet.Balance);
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
            _logger.Info("Player {PlayerId} {Currency} wallet balance set to {Balance}", playerId, currency, newBalance);
        }

        public void Deposit(int playerId, Currency currency, decimal amount)
        {
            var wallet = GetWallet(playerId, currency);
            wallet.Deposit(amount);
            _context.SaveChanges();
            _logger.Info("Deposited {Amount} to player {PlayerId} {Currency} wallet (balance {Balance})", amount, playerId, currency, wallet.Balance);
        }

        public void Withdraw(int playerId, Currency currency, decimal amount)
        {
            var wallet = GetWallet(playerId, currency);
            wallet.Withdraw(amount);
            _context.SaveChanges();
            _logger.Info("Withdrew {Amount} from player {PlayerId} {Currency} wallet (balance {Balance})", amount, playerId, currency, wallet.Balance);
        }

        public void Block(int playerId, Currency currency)
        {
            var wallet = GetWallet(playerId, currency);
            wallet.Block();
            _context.SaveChanges();
            _logger.Info("Player {PlayerId} {Currency} wallet blocked", playerId, currency);
        }

        public void Unblock(int playerId, Currency currency)
        {
            var wallet = GetWallet(playerId, currency);
            wallet.Unblock();
            _context.SaveChanges();
            _logger.Info("Player {PlayerId} {Currency} wallet unblocked", playerId, currency);
        }
    }
}