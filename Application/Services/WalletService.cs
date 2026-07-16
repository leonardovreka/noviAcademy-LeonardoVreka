using Application.Interfaces;
using Application.Strategies;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class WalletService : IWalletService
{
    private readonly IWalletRepository _walletRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly ILogger<WalletService> _logger;
    private readonly IReadOnlyDictionary<FundsOperation, IFundsStrategy> _fundsStrategies;

    public WalletService(
        IWalletRepository walletRepository,
        IPlayerRepository playerRepository,
        IEnumerable<IFundsStrategy> strategies,
        ILogger<WalletService> logger)
    {
        _walletRepository = walletRepository;
        _playerRepository = playerRepository;
        _logger = logger;
        _fundsStrategies = strategies.ToDictionary(strategy => strategy.Operation);
    }

    public async Task AddWalletToPlayer()
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        var currency = Prompts.PromptCurrency();
        if (currency is null)
            return;

        var balance = Prompts.PromptAmount("Initial balance");
        if (balance is null)
            return;

        try
        {
            if (await _playerRepository.FindPlayer(playerId.Value) is null)
                throw new PlayerNotFoundException(playerId.Value);

            var wallet = new Wallet(playerId.Value, currency.Value, balance.Value);
            await _walletRepository.Add(wallet);
            Console.WriteLine("Wallet added successfully.");
        }
        catch (PlayerNotFoundException ex)
        {
            _logger.LogWarning(ex, "Could not add wallet, player {PlayerId} not found", playerId);
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (WalletException ex)
        {
            _logger.LogWarning(ex, "Could not add wallet for player {PlayerId} in {Currency}", playerId, currency);
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public async Task GetWalletsOfPlayer()
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        var wallets = await _walletRepository.GetAllWalletsByPlayerId(playerId.Value);

        if (wallets.Count == 0)
        {
            Console.WriteLine("No wallets found for this player.");
            return;
        }

        foreach (var wallet in wallets)
            Console.WriteLine($"Wallet Number {wallets.IndexOf(wallet)} {wallet}");
    }

    public async Task DepositToWallet()
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        var currency = Prompts.PromptCurrency();
        if (currency is null)
            return;

        var amount = Prompts.PromptAmount("Amount to deposit");
        if (amount is null)
            return;

        await RunWalletOperation(async () =>
        {
            await _walletRepository.Deposit(playerId.Value, currency.Value, amount.Value);
            Console.WriteLine("Deposit successful.");
        });
    }

    public async Task WithdrawFromWallet()
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        var currency = Prompts.PromptCurrency();
        if (currency is null)
            return;

        var amount = Prompts.PromptAmount("Amount to withdraw");
        if (amount is null)
            return;

        await RunWalletOperation(async () =>
        {
            await _walletRepository.Withdraw(playerId.Value, currency.Value, amount.Value);
            Console.WriteLine("Withdrawal successful.");
        });
    }

    public async Task BlockWallet()
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        var currency = Prompts.PromptCurrency();
        if (currency is null)
            return;

        await RunWalletOperation(async () =>
        {
            await _walletRepository.Block(playerId.Value, currency.Value);
            Console.WriteLine("Wallet blocked.");
        });
    }

    public async Task UnblockWallet()
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        var currency = Prompts.PromptCurrency();
        if (currency is null)
            return;

        await RunWalletOperation(async () =>
        {
            await _walletRepository.Unblock(playerId.Value, currency.Value);
            Console.WriteLine("Wallet unblocked.");
        });
    }

    public async Task UpdateWalletBalance()
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        var currency = Prompts.PromptCurrency();
        if (currency is null)
            return;

        var newBalance = Prompts.PromptAmount("New balance");
        if (newBalance is null)
            return;

        await RunWalletOperation(async () =>
        {
            await _walletRepository.UpdateBalance(playerId.Value, currency.Value, newBalance.Value);
            Console.WriteLine("Balance updated.");
        });
    }

    public async Task ApplyFundsStrategy()
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        var currency = Prompts.PromptCurrency();
        if (currency is null)
            return;

        var operation = Prompts.PromptFundsOperation();
        if (operation is null)
            return;

        var amount = Prompts.PromptAmount("Amount");
        if (amount is null)
            return;

        var strategy = _fundsStrategies[operation.Value];

        await RunWalletOperation(async () =>
        {
            var wallet = await _walletRepository.GetWallet(playerId.Value, currency.Value);
            strategy.Execute(wallet, amount.Value);
            _logger.LogInformation("Applied {Strategy} of {Amount} to player {PlayerId} {Currency} wallet (balance {Balance})",
                strategy.GetType().Name, amount, playerId, currency, wallet.Balance);
            Console.WriteLine($"{operation} operation applied.");
        });
    }

    private async Task RunWalletOperation(Func<Task> operation)
    {
        try
        {
            await operation();
        }
        catch (WalletException ex)
        {
            _logger.LogWarning(ex, "Wallet operation failed");
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public async Task<Wallet> CreateWallet(int playerId, Currency currency, decimal initialBalance, CancellationToken ct = default)
    {
        var player = await _playerRepository.FindPlayer(playerId, ct);
        if (player is null) throw new PlayerNotFoundException(playerId);

        var wallet = new Wallet(playerId, currency, initialBalance);
        await _walletRepository.Add(wallet, ct);
        return wallet;
    }

    public Task<Wallet> GetWallet(int playerId, Currency currency, CancellationToken ct = default)
        => _walletRepository.GetWallet(playerId, currency, ct);

    public Task Deposit(int playerId, Currency currency, decimal amount, CancellationToken ct = default)
        => _walletRepository.Deposit(playerId, currency, amount, ct);
}