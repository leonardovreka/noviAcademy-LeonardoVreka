using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Queries.Wallets;

public class GetWalletQueryHandler : IRequestHandler<GetWalletQuery, Wallet>
{
    private readonly IWalletRepository _walletRepository;

    public GetWalletQueryHandler(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public Task<Wallet> Handle(GetWalletQuery request, CancellationToken cancellationToken)
        => _walletRepository.GetWallet(request.PlayerId, request.Currency, cancellationToken);
}