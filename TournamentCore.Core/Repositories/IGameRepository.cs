using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentCore.Core.Entities;

namespace TournamentCore.Core.Repositories
{
    public interface IGameRepository
    {
        Task<IEnumerable<GameEntities>> GetAllAsync();
        Task<IEnumerable<GameEntities>> GetByTitleAsync(string title);
        Task<GameEntities> GetAsync(int id);
        Task<bool> AnyAsync(int id);
        void Add(GameEntities game);
        void Update(GameEntities game);
        void Remove(GameEntities game);
    }
}
