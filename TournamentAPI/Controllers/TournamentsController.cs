using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Azure;
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
    public class TournamentsController : ControllerBase
    {
        private readonly IUoW _uow;
        private readonly IMapper _mapper;

        public TournamentsController(IUoW uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] bool includeGames = false)
        {
            IEnumerable<TournamentEntities> tournaments;
            if (includeGames)
            {
                tournaments = await _uow.TournamentRepository.GetAllIncludingGamesAsync();
            }
            else
            {
                tournaments = await _uow.TournamentRepository.GetAllAsync();
            }
            
            var tournamentDtos = _mapper.Map<IEnumerable<TournamentDto>>(tournaments);
            return Ok(tournamentDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTournament(int id)
        {
            var tournament = await _uow.TournamentRepository.GetAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }

            var tournamentDto = _mapper.Map<TournamentDto>(tournament);
            return Ok(tournamentDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournament(int id, TournamentDto tournamentDto)
        {
            if (id != tournamentDto.Id)
            {
                return BadRequest();
            }

            var tournament = _mapper.Map<TournamentEntities>(tournamentDto);
            _uow.TournamentRepository.Update(tournament);

            try
            {
                await _uow.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TournamentExists(id))
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
        public async Task<IActionResult> PostTournament([FromBody] TournamentDto tournamentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tournament = _mapper.Map<TournamentEntities>(tournamentDto);
            _uow.TournamentRepository.Add(tournament);
            try
            {
                await _uow.CompleteAsync();
            }
            catch
            {
                return StatusCode(500, "Failed to save changes to the database.");
            }
            

            var createdTournamentDto = _mapper.Map<TournamentDto>(tournament);
            return CreatedAtAction("GetTournament", new { id = createdTournamentDto.Id }, createdTournamentDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            var tournament = await _uow.TournamentRepository.GetAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }

            _uow.TournamentRepository.Remove(tournament);
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


        [HttpPatch("{tournamentId}")]
        public async Task<ActionResult<TournamentDto>> PatchTournament(int tournamentId, [FromBody] JsonPatchDocument<TournamentDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var tournament = await _uow.TournamentRepository.GetAsync(tournamentId);
            if (tournament == null)
            {
                return NotFound();
            }

            var tournamentDto = _mapper.Map<TournamentDto>(tournament);

            patchDocument.ApplyTo(tournamentDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(tournamentDto, tournament);
            _uow.TournamentRepository.Update(tournament);

            try
            {
                await _uow.CompleteAsync();
            }
            catch
            {
                return StatusCode(500, "Failed to save changes to the database.");
            }

            return Ok(_mapper.Map<TournamentDto>(tournament));
        }



        private async Task<bool> TournamentExists(int id)
        {
            return await _uow.TournamentRepository.AnyAsync(id);
        }
    }
}

