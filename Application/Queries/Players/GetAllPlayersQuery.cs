using Domain.Entities;
using MediatR;

namespace Application.Queries.Players;

public record GetAllPlayersQuery : IRequest<IEnumerable<Player>>;