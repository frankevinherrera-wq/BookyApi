using Microsoft.AspNetCore.Mvc;
using BookfyApi.DTOs.Libros;
using BookfyApi.Services.Interfaces;
using BookfyApi.Services.Implementations;



namespace BookfyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibrosController : ControllerBase
{
    private readonly ILibroService _libroService;
    public LibrosController(ILibroService libroService)
    {
        _libroService = libroService;
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var libros = await _libroService.GetAllAsync();
        return Ok(libros);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var libro = await _libroService.GetByIdAsync(id);
        if (libro == null) return NotFound("no se encuentra el id");
        return Ok(libro);
    }

    [HttpPost]
    public async Task<IActionResult> Create(LibroCreateDto dto)
    {
        try
        {
            var libro = await _libroService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = libro.Id }, libro);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, LibroUpdateDto dto)
    {

        try
        {
            
            var update = await _libroService.UpdateAsync(id, dto);
            if (!update) 
                return NotFound(new { mensaje = $"No se encontró el libro con ID {id} para actualizar." });

            return NoContent();

        }

        catch(ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });        
        }
       
    }



    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var del = await _libroService.DeleteAsync(id);
        if (!del) return NotFound("nose encontro");

       return NoContent();
    }


    
    [HttpGet("categoria/{categoriaId}")]
    public async Task<IActionResult> GetCategorias(int categoriaId){

        var cat = await _libroService.GetByCategoriaAsync(categoriaId);
        return Ok(cat);
    }


    [HttpGet("autor/{autorId}")]

    public async Task<IActionResult> GetByAutor(int autorId)
    {
        var autor = await _libroService.GetByAutorAsync(autorId);
        return Ok(autor);
    }



    [HttpGet("buscar")]
    public async Task<IActionResult> SearchByTitulo(string titulo)
    {
        var libros = await _libroService.SearchByTituloAsync(titulo);
        return Ok(libros);
    }

}