using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IPlayerRepository _playerRepository;

        public WalletsController(IWalletRepository walletRepository, IPlayerRepository playerRepository)
        {
            _walletRepository = walletRepository;
            _playerRepository = playerRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWalletRequest request, CancellationToken ct)
        {
            try
            {
                var player = await _playerRepository.FindPlayer(request.PlayerId, ct);
                if (player is null) return NotFound($"Player {request.PlayerId} not found");

                var wallet = new Wallet(request.PlayerId, request.Currency, request.InitialBalance);
                await _walletRepository.Add(wallet, ct);

                return CreatedAtAction(nameof(GetById), new { playerId = request.PlayerId, currency = request.Currency }, wallet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{playerId:int}/{currency}")]
        public async Task<IActionResult> GetById(int playerId, Currency currency, CancellationToken ct)
        {
            try
            {
                var wallet = await _walletRepository.GetWallet(playerId, currency, ct);
                return Ok(wallet);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{playerId:int}/{currency}/deposit")]
        public async Task<IActionResult> Deposit(int playerId, Currency currency, [FromBody] DepositRequest request, CancellationToken ct)
        {
            try
            {
                await _walletRepository.Deposit(playerId, currency, request.Amount, ct);
                var wallet = await _walletRepository.GetWallet(playerId, currency, ct);
                return Ok(wallet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public record CreateWalletRequest(int PlayerId, Currency Currency, decimal InitialBalance);
    public record DepositRequest(decimal Amount);
}