using Microsoft.AspNetCore.Mvc;
using BookfyApi.DTOs.Categoria;
using BookfyApi.Services.Interfaces;



namespace BookfyApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;
    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categorias = await _categoriaService.GetAllAsync();
        return Ok(categorias);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var categoria = await _categoriaService.GetByIdAsync(id);

        if (categoria == null) return NotFound("no encontrado");

        return Ok(categoria); 
    }


    [HttpPost]
    public async Task<IActionResult> Create(CategoriaCreateDto dto)
    {
        var nuevaCategoria = await _categoriaService.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById), new {id= nuevaCategoria.Id}, nuevaCategoria); 
        
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CategoriaUpdateDto dto)
    {
        var exito = await _categoriaService.UpdateAsync(id, dto);
        if (!exito) return NotFound("La categoría no existe"); 

        return NoContent(); 
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var exito = await _categoriaService.DeleteAsync(id);
        if (!exito) return NotFound("La categoría no existe"); 

        return NoContent(); 
    }


}