using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TournamentCore.Core.Entities;
using TournamentCore.Core.Repositories;
using TournamentData.Data.Data;

namespace TournamentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        //Uppdaterar _uow i alla methoder och tar bort dbcontext

        //private readonly TournamentApiContext _context;

        //public TournamentsController(TournamentApiContext context)
        //{
        //    _context = context;
        //}

        private readonly IUoW _uow;
        public TournamentsController(IUoW uow)
        {
            this._uow = uow;
        }

        //// GET: api/Tournaments
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<TournamentEntities>>> GetTournaments()
        //{
        //    return await _context.Tournaments.ToListAsync();
        //}

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tournaments = await _uow.TournamentRepository.GetAllAsync();
            return Ok(tournaments);
        }

        // GET: api/Tournaments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentEntities>> GetTournament(int id)
        {
            //var tournament = await _context.Tournaments.FindAsync(id);
            var tournament = await _uow.TournamentRepository.GetAsync(id);

            if (tournament == null)
            {
                return NotFound();
            }

            return tournament;
        }

        // PUT: api/Tournaments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournament(int id, TournamentEntities tournament)
        {
            if (id != tournament.Id)
            {
                return BadRequest();
            }

            //_context.Entry(tournament).State = EntityState.Modified;
            _uow.TournamentRepository.Update(tournament);

            try
            {
                //await _context.SaveChangesAsync();
                await _uow.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _uow.TournamentRepository.AnyAsync(id))
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

        // POST: api/Tournaments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TournamentEntities>> PostTournament(TournamentEntities tournament)
        {
            //_context.Tournaments.Add(tournament);
            _uow.TournamentRepository.Add(tournament);
            //await _context.SaveChangesAsync();
            await _uow.CompleteAsync();

            return CreatedAtAction("GetTournament", new { id = tournament.Id }, tournament);
        }

        // DELETE: api/Tournaments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            //var tournament = await _context.Tournaments.FindAsync(id);
            var tournament = await _uow.TournamentRepository.GetAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }

            //_context.Tournaments.Remove(tournament);
            _uow.TournamentRepository.Remove(tournament);
            //await _context.SaveChangesAsync();
            await _uow.CompleteAsync();

            return NoContent();
        }

        //private bool TournamentExists(int id)
        //{
        //    return _context.Tournaments.Any(e => e.Id == id);
        //}
        private async Task<bool> TournamentExists(int id)
        {
            return await _uow.TournamentRepository.AnyAsync(id);
        }
    }
}
