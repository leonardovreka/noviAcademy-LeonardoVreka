namespace WorldRank;

public interface IPlayer
{
    int Id { get; }
    string Name { get; }
    int Score { get; }
    IReadOnlyCollection<Wallet> Wallets { get; }
    void AddWallet(Wallet wallet);
}

public class Player : IPlayer
{
    public int Id { get; }
    public string Name { get; set; }
    public int Score { get; private set; }

    private readonly List<Wallet> _wallets = new();
    public IReadOnlyCollection<Wallet> Wallets => _wallets.AsReadOnly();

    public Player(int id, string name)
	{
		if (string.IsNullOrEmpty(name))
			throw new ArgumentException("Name cannot be null or empty.", nameof(name));

		Id = id;
		Name = name;
	}

    public void AddWallet(Wallet wallet)
    {
        if (_wallets.Any(w => w.Currency == wallet.Currency))
            throw new InvalidOperationException($"Player already has a {wallet.Currency} wallet.");
        
        _wallets.Add(wallet);
    }

    public void UpdateScore(int newScore)
	{
		if (newScore < 0)
			throw new ArgumentOutOfRangeException(nameof(newScore), "Score cannot be negative.");

		Score = newScore;
	}

	public override string ToString() =>
			$"[{Id}] {Name} - Score: {Score}";
}