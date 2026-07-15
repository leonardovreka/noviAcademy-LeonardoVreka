using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Queries.Players;

public class GetPlayerQueryHandler : IRequestHandler<GetPlayerQuery, Player?>
{
    private readonly IPlayerRepository _playerRepository;

    public GetPlayerQueryHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public Task<Player?> Handle(GetPlayerQuery request, CancellationToken cancellationToken)
        => _playerRepository.FindPlayer(request.PlayerId, cancellationToken);
}