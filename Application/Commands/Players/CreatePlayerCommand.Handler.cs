using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Commands.Players;

public class CreatePlayerCommandHandler : IRequestHandler<CreatePlayerCommand, int>
{
    private readonly IPlayerRepository _playerRepository;

    public CreatePlayerCommandHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<int> Handle(CreatePlayerCommand request, CancellationToken cancellationToken)
    {
        var player = new Player(request.Name);
        player.AddScore(request.Score);
        await _playerRepository.AddPlayer(player, cancellationToken);
        return player.Id;
    }
}