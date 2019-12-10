using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backlog.Models
{
    public class MovieList
    {
        public int MovieListId { get; set; }
        public int MovieId { get; set; }
        public int ListId { get; set; }
        public Movie Movie { get; set; }
        public List List { get; set;  }
    }
}
