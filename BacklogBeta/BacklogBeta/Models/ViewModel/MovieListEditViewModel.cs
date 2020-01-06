using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BacklogBeta.Models.ViewModel
{
    public class MovieListEditViewModel
    {


        public ICollection<MovieList> MovieList { get; set; }
        public MovieList MovieListInstance { get; set; }
        public Movie Movie { get; set; }
        public List<Movie> AllMovies { get; set; } = new List<Movie>();
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public List List { get; set; }
        public List<int> SelectedMovies { get; set; } = new List<int>();
      
        public List<SelectListItem> AllMovieOptions
        {
            get
            {
                if (AllMovies == null) return null;

                return AllMovies
                    .Select(mo => new SelectListItem(mo.Title, mo.MovieId.ToString()))
                    .ToList();
            }
    }

        
    }
}
