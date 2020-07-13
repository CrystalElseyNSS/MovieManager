using MovieManager.Data;
using MovieManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace GenreManager.Repositories
{
    public class GenreRepository
    {
        private readonly ApplicationDbContext _context;

        public GenreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Genre> GetAll()
        {
            return _context.Genre.ToList();
        }
        public Genre GetById(int id)
        {
            return _context.Genre.FirstOrDefault(m => m.Id == id);
        }
    }
}
