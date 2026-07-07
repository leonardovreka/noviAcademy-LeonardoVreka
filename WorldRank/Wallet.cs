namespace WorldRank;

public class Wallet
{
    public decimal Balance { get; private set; }
    public Currency Currency { get; }
    public bool IsBlocked { get; private set; }
    
    public Wallet(Currency currency)
    {
        Currency = currency;
        Balance = 0;
        IsBlocked = false;
    }

    public void Deposit(decimal amount)
    {
        if (IsBlocked)
            throw new InvalidOperationException("Cannot deposit to a blocked wallet.");

        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Deposit amount must be positive.");

        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (IsBlocked)
            throw new InvalidOperationException("Cannot withdraw from a blocked wallet.");

        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Withdrawal amount must be positive.");

        if (amount > Balance)
            throw new InvalidOperationException("Insufficient funds for withdrawal.");

        Balance -= amount;
    }

    public void Block()
    {
        IsBlocked = true;
    }
    public void Unblock()
    {
        IsBlocked = false;
    }

    public override string ToString() =>
        $"Wallet - Currency: {Currency}, Balance: {Balance}";
}