using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookfyApi.Data;   // <-- ESTA LÍNEA ES CRUCIAL (con la D mayúscula)
using BookfyApi.Models; // <-- Para que encuentre a Autor y Libro// <-- Y esta también/ Apuntamos a la nueva ubicación del modelo Autor

namespace BookfyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutoresController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AutoresController(ApplicationDbContext context)
    {
        _context = context;
    }

    // 1. GET: api/autores (Listar todos)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Autor>>> GetAutores()
    {
        return await _context.Autores.ToListAsync();
    }

    // 2. GET: api/autores/5 (Obtener uno solo por ID)
    [HttpGet("{id}")]
    public async Task<ActionResult<Autor>> GetAutor(int id)
    {
        var autor = await _context.Autores.FindAsync(id);

        if (autor == null)
        {
            return NotFound(new { mensaje = "El autor no existe." });
        }

        return autor;
    }

    // 3. POST: api/autores (Crear nuevo autor)
    [HttpPost]
    public async Task<ActionResult<Autor>> PostAutor(Autor autor)
    {
        // El [ApiController] valida automáticamente los Data Annotations antes de entrar aquí
        _context.Autores.Add(autor);
        await _context.SaveChangesAsync();

        // Devuelve un código 201 Created y la URL para consultar el nuevo recurso
        return CreatedAtAction(nameof(GetAutor), new { id = autor.Id }, autor);
    }

    // 4. PUT: api/autores/5 (Actualizar autor existente)
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAutor(int id, Autor autor)
    {
        if (id != autor.Id)
        {
            return BadRequest(new { mensaje = "El ID del parámetro no coincide con el del cuerpo." });
        }

        // Le avisamos a Entity Framework que el objeto fue modificado
        _context.Entry(autor).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Autores.Any(e => e.Id == id))
            {
                return NotFound(new { mensaje = "El autor a actualizar ya no existe." });
            }
            throw;
        }

        return NoContent(); // Código 204 (Se actualizó con éxito, no devuelve contenido)
    }
}