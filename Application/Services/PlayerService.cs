using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class PlayerService
{
    private readonly IPlayerRepository _playerRepository;

    public PlayerService(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task AddPlayer()
    {
        Console.Write("Name: ");
        var name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Name cannot be empty.");
            return;
        }

        Console.Write("Score: ");
        var scoreInput = Console.ReadLine();
        if (!int.TryParse(scoreInput, out var score))
        {
            Console.WriteLine("Score must be a whole number.");
            return;
        }

        var player = new Player(name);
        player.AddScore(score);
        await _playerRepository.AddPlayer(player);
        Console.WriteLine("Player added successfully.");
    }

    public async Task ListPlayers()
    {
        var all = (await _playerRepository.GetAllPlayers()).ToList();

        if (all.Count == 0)
        {
            Console.WriteLine("No players registered.");
            return;
        }

        foreach (var player in all)
            Console.WriteLine(player);
    }

    public async Task ListPlayersByScore()
    {
        var groups = (await _playerRepository.GroupPlayersByScore()).ToList();

        if (groups.Count == 0)
        {
            Console.WriteLine("No players registered.");
            return;
        }

        foreach (var group in groups)
        {
            Console.WriteLine($"Score {group.Key}:");
            foreach (var player in group)
                Console.WriteLine($"  {player}");
        }
    }

    public async Task FindPlayerByName()
    {
        Console.Write("Search by name: ");
        var term = Console.ReadLine() ?? string.Empty;

        var player = (await _playerRepository.GetAllPlayers())
            .FirstOrDefault(p => p.Name.Equals(term, StringComparison.OrdinalIgnoreCase));

        Console.WriteLine(player is null ? "No player found." : player.ToString());
    }

    public async Task FindPlayerById()
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        var player = await _playerRepository.FindPlayer(playerId.Value);

        Console.WriteLine(player is null ? "No player found." : player.ToString());
    }

    public async Task DeletePlayer()
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        await _playerRepository.DeletePlayer(playerId.Value);
        Console.WriteLine("Player deleted (if it existed).");
    }

    public async Task<Player> CreatePlayer(string name, int score, CancellationToken ct = default)
    {
        var player = new Player(name);
        player.AddScore(score);
        await _playerRepository.AddPlayer(player, ct);
        return player;
    }

    public Task<IEnumerable<Player>> GetAllPlayers(CancellationToken ct = default)
        => _playerRepository.GetAllPlayers(ct);

    public Task<Player?> FindPlayer(int playerId, CancellationToken ct = default)
        => _playerRepository.FindPlayer(playerId, ct);
}