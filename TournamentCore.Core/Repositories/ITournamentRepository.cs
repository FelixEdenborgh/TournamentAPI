using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentCore.Core.Entities;

namespace TournamentCore.Core.Repositories
{
    public interface ITournamentRepository
    {
        Task<IEnumerable<TournamentEntities>> GetAllAsync();
        Task<IEnumerable<TournamentEntities>> GetAllIncludingGamesAsync();
        Task<TournamentEntities> GetAsync(int id);
        Task<bool> AnyAsync(int id);
        void Add(TournamentEntities tournament);
        void Update(TournamentEntities tournament);
        void Remove(TournamentEntities tournament);

    }
}
