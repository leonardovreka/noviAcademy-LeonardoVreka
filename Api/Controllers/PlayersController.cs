using Api.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly PlayerService _playerService;

        public PlayersController(PlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePlayerRequest request, CancellationToken ct)
        {
            try
            {
                var player = await _playerService.CreatePlayer(request.Name, request.Score, ct);          
                return CreatedAtAction(nameof(GetById), new { playerId = player.Id }, player);
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
                var result = await _playerService.GetAllPlayers(ct);
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
                var result = await _playerService.FindPlayer(playerId, ct);
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