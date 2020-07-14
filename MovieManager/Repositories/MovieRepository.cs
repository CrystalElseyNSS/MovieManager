using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using MovieManager.Data;
using MovieManager.Models;

namespace MovieManager.Repositories
{
    public class MovieRepository
    {
        private readonly ApplicationDbContext _context;

        public MovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Movie> GetAll()
        {
            return _context.Movie.ToList();
        }

        public Movie GetById(int id)
        {
            return _context.Movie
                           .Include(m => m.Genre)
                           .FirstOrDefault(m => m.GenreId == id);
        }

        public void Add(Movie movie)
        {
            _context.Add(movie);
            _context.SaveChanges();
        }

        public void Update(Movie movie)
        {
            _context.Entry(movie).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var movie = GetById(id);
            _context.Remove(movie);
        }
    }
}
