using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class PlayerService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly ILogger<PlayerService> _logger;

    public PlayerService(IPlayerRepository playerRepository, ILogger<PlayerService> logger)
    {
        _playerRepository = playerRepository;
        _logger = logger;
    }

    public void AddPlayer()
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
        _playerRepository.AddPlayer(player);
        _logger.LogInformation("Added new player: {Name}, with score {Score}", player.Name, player.Score);
        Console.WriteLine("Player added successfully.");
    }

    public void ListPlayers()
    {
        var all = _playerRepository.GetAllPlayers().ToList();

        if (all.Count == 0)
        {
            Console.WriteLine("No players registered.");
            return;
        }

        foreach (var player in all)
            Console.WriteLine(player);
    }

    public void ListPlayersByScore()
    {
        var groups = _playerRepository.GroupPlayersByScore().ToList();

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

    public void FindPlayerByName()
    {
        Console.Write("Search by name: ");
        var term = Console.ReadLine() ?? string.Empty;

        var player = _playerRepository.GetAllPlayers()
            .FirstOrDefault(p => p.Name.Equals(term, StringComparison.OrdinalIgnoreCase));

        Console.WriteLine(player is null ? "No player found." : player.ToString());
    }

    public void FindPlayerById()
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        var player = _playerRepository.FindPlayer(playerId.Value);

        Console.WriteLine(player is null ? "No player found." : player.ToString());
    }

    public void DeletePlayer()
    {
        var playerId = Prompts.PromptPlayerId();
        if (playerId is null)
            return;

        if (_playerRepository.DeletePlayer(playerId.Value))
        {
            _logger.LogInformation("Player {PlayerId} deleted successfully.", playerId.Value);
            Console.WriteLine("Player deleted successfully.");
        }
        else
        {
            _logger.LogWarning("Delete skipped: player {PlayerId} not found.", playerId.Value);
            Console.WriteLine("No player found with the given ID.");
        }
    }

    // Generates a random, unique player id (avoids collisions with already-registered players).
    private int GeneratePlayerId()
    {
        var existingIds = _playerRepository.GetAllPlayers().Select(p => p.Id).ToHashSet();

        int id;
        do
        {
            id = Random.Shared.Next(1, int.MaxValue);
        }
        while (existingIds.Contains(id));

        return id;
    }
}
