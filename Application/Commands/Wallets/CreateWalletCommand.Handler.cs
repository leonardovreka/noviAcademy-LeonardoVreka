using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Commands.Wallets;

public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IPlayerRepository _playerRepository;

    public CreateWalletCommandHandler(IWalletRepository walletRepository, IPlayerRepository playerRepository)
    {
        _walletRepository = walletRepository;
        _playerRepository = playerRepository;
    }

    public async Task Handle(CreateWalletCommand request, CancellationToken cancellationToken)
    {
        var player = await _playerRepository.FindPlayer(request.PlayerId, cancellationToken);
        if (player is null) throw new PlayerNotFoundException(request.PlayerId);

        var wallet = new Wallet(request.PlayerId, request.Currency, request.InitialBalance);
        await _walletRepository.Add(wallet, cancellationToken);
    }
}