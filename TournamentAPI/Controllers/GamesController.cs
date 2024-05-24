using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TournamentCore.Core.Dto;
using TournamentCore.Core.Entities;
using TournamentCore.Core.Repositories;

namespace TournamentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IUoW _uow;
        private readonly IMapper _mapper;

        public GamesController(IUoW uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var games = await _uow.GameRepository.GetAllAsync();
            var gameDtos = _mapper.Map<IEnumerable<GameDto>>(games);
            return Ok(gameDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGame(int id)
        {
            var game = await _uow.GameRepository.GetAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            var gameDto = _mapper.Map<GameDto>(game);
            return Ok(gameDto);
        }

        [HttpGet("byTitle/{title}")]
        public async Task<IActionResult> GetGameByTitle(string title)
        {
            var games = await _uow.GameRepository.GetByTitleAsync(title);
            if (games == null || !games.Any())
            {
                return NotFound();
            }

            var gameDtos = _mapper.Map<IEnumerable<GameDto>>(games);
            return Ok(gameDtos);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, GameDto gameDto)
        {
            if (id != gameDto.Id)
            {
                return BadRequest();
            }

            var game = _mapper.Map<GameEntities>(gameDto);
            _uow.GameRepository.Update(game);

            try
            {
                await _uow.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500, "Failed to save changes to the database.");
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostGame([FromBody] GameDto gameDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tournamentExists = await _uow.TournamentRepository.AnyAsync(gameDto.TournamentId);
            if (!tournamentExists)
            {
                return BadRequest("Invalid TournamentId");
            }

            var game = _mapper.Map<GameEntities>(gameDto);
            _uow.GameRepository.Add(game);
            try
            {
                await _uow.CompleteAsync();
            }
            catch
            {
                return StatusCode(500, "Failed to save changes to the database.");
            }
            

            var createdGameDto = _mapper.Map<GameDto>(game);
            return CreatedAtAction(nameof(GetGame), new { id = createdGameDto.Id }, createdGameDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _uow.GameRepository.GetAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _uow.GameRepository.Remove(game);
            try
            {
                await _uow.CompleteAsync();
            }
            catch
            {
                return StatusCode(500, "Failed to save changes to the database.");
            }
            

            return NoContent();
        }


        [HttpPatch("{gameId}")]
        public async Task<ActionResult<GameDto>> PatchGame(int gameId, [FromBody] JsonPatchDocument<GameDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var game = await _uow.GameRepository.GetAsync(gameId);
            if (game == null)
            {
                return NotFound();
            }

            var gameDto = _mapper.Map<GameDto>(game);

            patchDocument.ApplyTo(gameDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(gameDto, game);
            _uow.GameRepository.Update(game);

            try
            {
                await _uow.CompleteAsync();
            }
            catch
            {
                return StatusCode(500, "Failed to save changes to the database.");
            }

            return Ok(_mapper.Map<GameDto>(game));
        }

        private async Task<bool> GameExists(int id)
        {
            return await _uow.GameRepository.AnyAsync(id);
        }
    }
}
