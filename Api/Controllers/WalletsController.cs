using Application.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Api.DTOs;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalletsController : ControllerBase
    {
        private readonly WalletService _walletService;

        public WalletsController(WalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWalletRequest request, CancellationToken ct)
        {
            try
            {
                var wallet = await _walletService.CreateWallet(request.PlayerId, request.Currency, request.InitialBalance, ct);
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
                var wallet = await _walletService.GetWallet(playerId, currency, ct);
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
                var wallet = await _walletService.Deposit(playerId, currency, request.Amount, ct);
                return Ok(wallet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}