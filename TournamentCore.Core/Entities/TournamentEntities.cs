﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentCore.Core.Entities
{
    public class TournamentEntities
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime StartDate { get; set; }
        public ICollection<GameEntities>? Games { get; set; }
    }
}