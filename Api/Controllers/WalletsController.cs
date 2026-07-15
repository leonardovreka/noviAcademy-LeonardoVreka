using Api.DTOs;
using Application.Commands.Wallets;
using Application.Queries.Wallets;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalletsController : ControllerBase
    {
        private readonly ISender _mediator;

        public WalletsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWalletRequest request, CancellationToken ct)
        {
            try
            {
                await _mediator.Send(new CreateWalletCommand(request.PlayerId, request.Currency, request.InitialBalance), ct);
                var wallet = await _mediator.Send(new GetWalletQuery(request.PlayerId, request.Currency), ct);
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
                var wallet = await _mediator.Send(new GetWalletQuery(playerId, currency), ct);
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
                await _mediator.Send(new DepositCommand(playerId, currency, request.Amount), ct);
                var wallet = await _mediator.Send(new GetWalletQuery(playerId, currency), ct);
                return Ok(wallet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}