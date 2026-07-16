using Domain.Enums;
using MediatR;

namespace Application.Commands.Wallets;

public record CreateWalletCommand(int PlayerId, Currency Currency, decimal InitialBalance) : IRequest;