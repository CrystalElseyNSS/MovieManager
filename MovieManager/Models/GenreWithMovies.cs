using System.Collections.Generic;

namespace MovieManager.Models
{
    public class GenreWithMovies
    {
        public GenreWithMovies()
        {
            Movies = new List<Movie>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int MovieCount
        {
            get
            {
                return Movies.Count;
            }
        }
        public List<Movie> Movies { get; set; }
    }
}
