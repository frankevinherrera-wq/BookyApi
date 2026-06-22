using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookfyApi.Data;
using BookfyApi.Models;
using BookfyApi.DTOs;

namespace BookfyApi.Controllers
{
    [ApiController] 
    [Route("api/[controller]")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LibrosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibroDto>>> GetLibros()
        {
            return await _context.Libros
                .Include(l => l.Autor) 
                .Select(libro => new LibroDto
                {
                    Titulo = libro.Titulo,
                    AnioPublicacion = libro.AnioPublicacion,
                    Autor = new AutorDto // Transforma la relación en un DTO limpio
                    {
                        Nombre = libro.Autor != null ? libro.Autor.Nombre : "Sin Autor",
                        Nacionalidad = libro.Autor != null ? libro.Autor.Nacionalidad : "Desconocida"
                    }
                }).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LibroDto>> GetLibro(int id)
        {
            var libro = await _context.Libros
                .Include(l => l.Autor)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (libro == null)
            {
                return NotFound(new { mensaje = "El libro no existe." });
            }

            return new LibroDto
            {
                Titulo = libro.Titulo,
                AnioPublicacion = libro.AnioPublicacion,
                Autor = new AutorDto
                {
                    Nombre = libro.Autor != null ? libro.Autor.Nombre : "Sin Autor",
                    Nacionalidad = libro.Autor != null ? libro.Autor.Nacionalidad : "Desconocida"
                }
            };
        }

        [HttpPost]
        public async Task<ActionResult<LibroDto>> PostLibro(LibroDto libroDto)
        {
            var autorExiste = await _context.Autores.AnyAsync(a => a.Id == libroDto.AutorId);
            if (!autorExiste)
            {
                return BadRequest(new { mensaje = "El AutorId proporcionado no existe." });
            }

            var libro = new Libro
            {
                Titulo = libroDto.Titulo,
                AnioPublicacion = libroDto.AnioPublicacion,
                AutorId = libroDto.AutorId ?? 0
            };

            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLibro), new { id = libro.Id }, libroDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLibro(int id, LibroDto libroDto)
        {
            var libroDb = await _context.Libros.FindAsync(id);
            if (libroDb == null)
            {
                return NotFound(new { mensaje = "El libro a actualizar no existe." });
            }

            var autorExiste = await _context.Autores.AnyAsync(a => a.Id == libroDto.AutorId);
            if (!autorExiste)
            {
                return BadRequest(new { mensaje = "El AutorId proporcionado no existe." });
            }

            libroDb.Titulo = libroDto.Titulo;
            libroDb.AnioPublicacion = libroDto.AnioPublicacion;
            libroDb.AutorId = libroDto.AutorId ?? 0;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // 5. DELETE - Impecable, estaba muy bien estructurado
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibro(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound(new { mensaje = "El libro a eliminar no existe." });
            }

            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}