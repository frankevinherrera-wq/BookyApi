
using Microsoft.AspNetCore.Mvc;
using BookfyApi.DTOs.Autor;
using BookfyApi.Services.Interfaces;


namespace BookfyApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AutoresController : ControllerBase
{
    private readonly IAutorService _autorService;
    public AutoresController(IAutorService autorService)
    {
        _autorService = autorService;
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var autores = await _autorService.GetAllAsync();

        return Ok(autores);
    }

    

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var autor = await _autorService.GetByIdAsync(id);
        if (autor == null) NotFound("autor no encontrado");


        return Ok(autor);

    }

    [HttpPost]
    public async Task<IActionResult> Create(AutorCreateDto dto)
    {
        var nuevo = await _autorService.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById), new {id = nuevo.Id} , nuevo);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> update(int id , AutorUpdateDto dto)
    {
        var actual = await _autorService.UpdateAsync(id, dto);
        if (!actual)return NotFound("autor no actualizado");
        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var exito = await _autorService.DeleteAsync(id);
        if (!exito) return NotFound("el autor no existe"); 

        return NoContent(); 
    }



}