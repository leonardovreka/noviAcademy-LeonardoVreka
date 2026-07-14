using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace Infrastructure.Repositories
{
    public class DBWalletRepository : IWalletRepository
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly WorldRankDbContext _context;

        public DBWalletRepository(WorldRankDbContext context)
        {
            _context = context;
        }

        public async Task Add(Wallet wallet, CancellationToken ct = default)
        {
            var exists = await _context.Wallets.AnyAsync(w => w.PlayerId == wallet.PlayerId && w.Currency == wallet.Currency, ct);

            if (exists)
                throw new DuplicateWalletException(wallet.PlayerId, wallet.Currency);

            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync(ct);
            _logger.Info("Wallet created for player {PlayerId} in {Currency} with balance {Balance}", wallet.PlayerId, wallet.Currency, wallet.Balance);
        }

        public async Task<List<Wallet>> GetAllWalletsByPlayerId(int playerId, CancellationToken ct = default)
        {
            return await _context.Wallets
                .Where(w => w.PlayerId == playerId)
                .ToListAsync(ct);
        }

        public async Task<Wallet> GetWallet(int playerId, Currency currency, CancellationToken ct = default)
        {
            var wallet = await _context.Wallets
                .SingleOrDefaultAsync(w => w.PlayerId == playerId && w.Currency == currency, ct);

            if (wallet is null)
                throw new WalletNotFoundException(playerId, currency);

            return wallet;
        }

        public async Task UpdateBalance(int playerId, Currency currency, decimal newBalance, CancellationToken ct = default)
        {
            var wallet = await GetWallet(playerId, currency, ct);
            wallet.SetBalance(newBalance);
            await _context.SaveChangesAsync(ct);
            _logger.Info("Player {PlayerId} {Currency} wallet balance set to {Balance}", playerId, currency, newBalance);
        }

        public async Task Deposit(int playerId, Currency currency, decimal amount, CancellationToken ct = default)
        {
            var wallet = await GetWallet(playerId, currency, ct);
            wallet.Deposit(amount);
            await _context.SaveChangesAsync(ct);
        }

        public async Task Withdraw(int playerId, Currency currency, decimal amount, CancellationToken ct = default)
        {
            var wallet = await GetWallet(playerId, currency, ct);
            wallet.Withdraw(amount);
            await _context.SaveChangesAsync(ct);
            _logger.Info("Withdrew {Amount} from player {PlayerId} {Currency} wallet", amount, playerId, currency);
        }

        public async Task Block(int playerId, Currency currency, CancellationToken ct = default)
        {
            var wallet = await GetWallet(playerId, currency, ct);
            wallet.Block();
            await _context.SaveChangesAsync(ct);
            _logger.Info("Player {PlayerId} {Currency} wallet blocked", playerId, currency);
        }

        public async Task Unblock(int playerId, Currency currency, CancellationToken ct = default)
        {
            var wallet = await GetWallet(playerId, currency, ct);
            wallet.Unblock();
            await _context.SaveChangesAsync(ct);
            _logger.Info("Player {PlayerId} {Currency} wallet unblocked", playerId, currency);
        }
    }
}