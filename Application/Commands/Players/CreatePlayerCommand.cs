using MediatR;

namespace Application.Commands.Players;

public record CreatePlayerCommand(string Name, int Score) : IRequest<int>;