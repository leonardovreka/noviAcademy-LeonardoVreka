using Api.DTOs;
using Application.Commands.Players;
using Application.Queries.Players;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly ISender _mediator;

        public PlayersController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePlayerRequest request, CancellationToken ct)
        {
            try
            {
                var playerId = await _mediator.Send(new CreatePlayerCommand(request.Name, request.Score), ct);
                var player = await _mediator.Send(new GetPlayerQuery(playerId), ct);
                return CreatedAtAction(nameof(GetById), new { playerId }, player);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(new GetAllPlayersQuery(), ct);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{playerId:int}")]
        public async Task<IActionResult> GetById(int playerId, CancellationToken ct)
        {
            try
            {
                var result = await _mediator.Send(new GetPlayerQuery(playerId), ct);
                if (result is null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}