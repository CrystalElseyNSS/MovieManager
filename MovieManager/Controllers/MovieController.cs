using Microsoft.AspNetCore.Mvc;
using MovieManager.Data;
using MovieManager.Models;
using MovieManager.Repositories;

namespace MovieManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieRepository _movieRepository;

        public MovieController(ApplicationDbContext context)
        {
            _movieRepository = new MovieRepository(context);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_movieRepository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var movie = _movieRepository.GetById(id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }
 
        [HttpPost]
        public IActionResult AddMovie(Movie movie)
        {
            _movieRepository.Add(movie);
            return CreatedAtAction(nameof(Get), new { id = movie.Id }, movie);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _movieRepository.Update(movie);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _movieRepository.Delete(id);
            return NoContent();
        }
    }
}
