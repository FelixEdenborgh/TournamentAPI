using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentCore.Core.Entities
{
    public class GameEntities
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Title length can't be more than 100.")]
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public int TournamentId { get; set; }
        public TournamentEntities Tournament { get; set; }
    }
}
