public class Player
{
    public Guid Id { get; }
    public string Name { get; set; }
    public int Score { get; private set; }

    public Player(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        Score = 0;
    }
}