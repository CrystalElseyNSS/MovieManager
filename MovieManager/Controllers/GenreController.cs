using GenreManager.Repositories;
using Microsoft.AspNetCore.Mvc;
using MovieManager.Data;

namespace MovieManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly GenreRepository _genreRepository;

        public GenreController(ApplicationDbContext context)
        {
            _genreRepository = new GenreRepository(context);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_genreRepository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var genre = _genreRepository.GetById(id);
            if (genre == null)
            {
                return NotFound();
            }
            return Ok(genre);
        }
    }
}
