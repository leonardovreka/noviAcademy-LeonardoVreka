using Application.Interfaces;
using MediatR;

namespace Application.Commands.Wallets;

public class DepositCommandHandler : IRequestHandler<DepositCommand>
{
    private readonly IWalletRepository _walletRepository;

    public DepositCommandHandler(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public Task Handle(DepositCommand request, CancellationToken cancellationToken)
        => _walletRepository.Deposit(request.PlayerId, request.Currency, request.Amount, cancellationToken);
}