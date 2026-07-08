namespace WorldRank;

public interface IPlayer
{
    int Id { get; }
    string Name { get; }
    int Score { get; }
    IReadOnlyCollection<IWallet> Wallets { get; }
    void AddWallet(IWallet wallet);
}

public class Player : IPlayer
{
    public int Id { get; }
    public string Name { get; private set; }
    public int Score { get; private set; }

    private readonly List<IWallet> _wallets = new();
    public IReadOnlyCollection<IWallet> Wallets => _wallets.AsReadOnly();

    public Player(int id, string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentException("Name cannot be null or empty.", nameof(name));

		Id = id;
		Name = name;
	}

    public void AddWallet(IWallet wallet)
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

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        throw new ArgumentException("Name cannot be null or empty.", nameof(newName));

        Name = newName;
    }

	public override string ToString() =>
			$"[{Id}] {Name} - Score: {Score}";
}