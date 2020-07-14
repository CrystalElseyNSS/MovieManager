using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using GenreManager.Repositories;
using MovieManager.Models;

namespace MovieManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly GenreRepository _genreRepository;

        public GenreController(IConfiguration configuration)
        {
            _genreRepository = new GenreRepository(configuration);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_genreRepository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id, bool includeMovies)
        {
            if (includeMovies)
            {
                var genre = _genreRepository.GetWithMoviesById(id);
                return Ok(genre);
            } 
            else
            {
                var genre = _genreRepository.GetById(id);
                return Ok(genre);
            }
        }

        [HttpPost]
        public IActionResult Post(Genre genre)
        {
            _genreRepository.Add(genre);
            return CreatedAtAction(nameof(Get), new { id = genre.Id }, genre);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Genre genre)
        {
            if (id != genre.Id)
            {
                return BadRequest();
            }
            _genreRepository.Update(genre);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            _genreRepository.Remove(id);
            return NoContent();
        }
    }
}
