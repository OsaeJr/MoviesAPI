using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


using MoviesAPI.Models;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly MovieContext _context;

        public MoviesController(MovieContext context)
        {
            _context = context;
        }

        // GET : api/Movies
        [HttpGet]

        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            if (_context.Movies == null)
                return NotFound();
            else
                return await _context.Movies.ToListAsync();
        }

        // GET : api/Movies/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            if (_context.Movies == null)
                return NotFound();

            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return NotFound();

            return movie;

        }

        // POST : api/Movies
        [HttpPost]

        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);

        }
        // GET : PUT/Movies/1
        [HttpPut]
        public async Task<ActionResult<Movie>> PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
                return BadRequest();


            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }

            catch(DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                    return NotFound();

                else
                    throw;
            }
            return NoContent();
        }

        private bool MovieExists(long id)
        {
            return (_context.Movies?.Any(movie => movie.Id == id)).GetValueOrDefault();
        }

        // DELETE : POST/Movies/1
        [HttpDelete]

        public async Task<ActionResult<Movie>> DeleteMovie(int id)
        {
           if(_context.Movies is null)
                return NotFound(); ;

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound();

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}