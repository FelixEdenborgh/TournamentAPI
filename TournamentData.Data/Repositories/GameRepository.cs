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
    public class GameRepository : IGameRepository
    {
        private readonly TournamentApiContext _context;
        public GameRepository(TournamentApiContext context)
        {

            _context = context;
        }

        public async Task<IEnumerable<GameEntities>> GetAllAsync() => await _context.Games.ToListAsync();
        public async Task<GameEntities> GetAsync(int id) => await _context.Games.FindAsync(id);

        public async Task<IEnumerable<GameEntities>> GetByTitleAsync(string title)
        {
            return await _context.Games.Where(g => g.Title == title).ToListAsync();
        }
        public async Task<bool> AnyAsync(int id) => await _context.Games.AnyAsync(g => g.Id == id);
        public void Add(GameEntities game) => _context.Games.Add(game);
        public void Update(GameEntities game) => _context.Games.Update(game);
        public void Remove(GameEntities game) => _context.Games.Remove(game);
    }
}
