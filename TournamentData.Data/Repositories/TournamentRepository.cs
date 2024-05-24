using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentCore.Core.Entities;
using TournamentCore.Core.Repositories;
using TournamentData.Data.Data;

namespace TournamentData.Data.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly TournamentApiContext _context;
        public TournamentRepository(TournamentApiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TournamentEntities>> GetAllAsync() => await _context.Tournaments.ToListAsync();

        public async Task<IEnumerable<TournamentEntities>> GetAllIncludingGamesAsync()
        {
            return await _context.Tournaments.Include(t => t.Games).ToListAsync();
        }
        public async Task<TournamentEntities> GetAsync(int id) => await _context.Tournaments.FindAsync(id);
        public async Task<bool> AnyAsync(int id) => await _context.Tournaments.AnyAsync(t => t.Id == id);
        public void Add(TournamentEntities tournament) => _context.Tournaments.Add(tournament);
        public void Update(TournamentEntities tournament) => _context.Tournaments.Update(tournament);
        public void Remove(TournamentEntities tournament) => _context.Tournaments.Remove(tournament);
    }
}
