using Domain.Enums;
using MediatR;

namespace Application.Commands.Wallets;

public record DepositCommand(int PlayerId, Currency Currency, decimal Amount) : IRequest;