using Domain.Entities;
using MediatR;

namespace Application.Queries.Players;

public record GetPlayerQuery(int PlayerId) : IRequest<Player?>;