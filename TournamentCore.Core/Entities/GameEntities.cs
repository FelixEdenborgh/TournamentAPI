using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentCore.Core.Entities
{
    public class GameEntities
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime Time { get; set; }
        public int TournamentId { get; set; }
        public TournamentEntities? Tournament { get; set; }
    }
}
