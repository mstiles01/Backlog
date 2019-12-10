
using BacklogActual.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BacklogActual.Models
{
    public class MovieList
    {
        [Key]
        public int MovieListId { get; set; }
        [Required]
        public int MovieId { get; set; }
        [Required]
        public int ListId { get; set; }

        public Movie Movie { get; set; }
        public List List { get; set; }
    }
}
