using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookfyApi.Data;  
using BookfyApi.Models; 
using BookfyApi.DTOs;
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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AutorDto>>> GetAutores()
    {
        return await _context.Autores
            .Select(autor => new AutorDto
            {
                Nombre = autor.Nombre,
                Nacionalidad = autor.Nacionalidad
            })
            .ToListAsync();
    }

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

    [HttpPost]
    public async Task<ActionResult<Autor>> PostAutor(Autor autor)
    {
        _context.Autores.Add(autor);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAutor), new { id = autor.Id }, autor);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAutor(int id, Autor autor)
    {
        if (id != autor.Id)
        {
            return BadRequest(new { mensaje = "El ID del parámetro no coincide con el del cuerpo." });
        }

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

        return NoContent(); 
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult<Autor>> DeleteAutor(int id)
    {
        var autor = await _context.Autores.FindAsync(id);
        if (autor == null)
        {
            return NotFound(new { mensaje = "El autor a eliminar no existe." });
        }

        _context.Autores.Remove(autor);
        await _context.SaveChangesAsync();

        return autor; 
    }
}