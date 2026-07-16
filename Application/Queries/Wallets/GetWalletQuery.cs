using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Queries.Wallets;

public record GetWalletQuery(int PlayerId, Currency Currency) : IRequest<Wallet>;