using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TournamentCore.Core.Entities;
using TournamentCore.Core.Repositories;

namespace TournamentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IUoW _uow;

        public GamesController(IUoW uow)
        {
            _uow = uow;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameEntities>>> GetGames()
        {
            var games = await _uow.GameRepository.GetAllAsync();
            return Ok(games);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameEntities>> GetGame(int id)
        {
            var game = await _uow.GameRepository.GetAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        // PUT: api/Games/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, GameEntities game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }

            var existingGame = await _uow.GameRepository.GetAsync(id);
            if (existingGame == null)
            {
                return NotFound();
            }

            // Update fields of the existing game entity
            existingGame.Title = game.Title;
            existingGame.Time = game.Time;
            existingGame.TournamentId = game.TournamentId;

            try
            {
                await _uow.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _uow.GameRepository.AnyAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Games
        [HttpPost]
        public async Task<ActionResult<GameEntities>> PostGame(GameEntities game)
        {
            var existingGame = await _uow.GameRepository.GetAsync(game.Id);
            if (existingGame != null)
            {
                return Conflict("A game with the same ID already exists.");
            }

            _uow.GameRepository.Add(game);
            await _uow.CompleteAsync();

            return CreatedAtAction("GetGame", new { id = game.Id }, game);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _uow.GameRepository.GetAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _uow.GameRepository.Remove(game);
            await _uow.CompleteAsync();

            return NoContent();
        }

        private async Task<bool> GameExists(int id)
        {
            return await _uow.GameRepository.AnyAsync(id);
        }
    }
}
