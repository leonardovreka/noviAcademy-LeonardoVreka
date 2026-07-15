using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Queries.Players;

public class GetAllPlayersQueryHandler : IRequestHandler<GetAllPlayersQuery, IEnumerable<Player>>
{
    private readonly IPlayerRepository _playerRepository;

    public GetAllPlayersQueryHandler(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public Task<IEnumerable<Player>> Handle(GetAllPlayersQuery request, CancellationToken cancellationToken)
        => _playerRepository.GetAllPlayers(cancellationToken);
}