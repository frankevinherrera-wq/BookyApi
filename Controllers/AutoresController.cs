using Microsoft.AspNetCore.Mvc;

using BookfyApi.DTOs;
using BookfyApi.Services;
namespace BookfyApi.Controllers
{
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
        public async Task<ActionResult<List<AutorReadDto>>> ListarTodos()
        {
            var autores = await _autorService.ListarTodos();
            return Ok(autores);
        }

        [HttpPost]
        public async Task<ActionResult<AutorReadDto>> Crear(AutorCreateDto autorDto)
        {
            var autorCreado = await _autorService.Crear(autorDto);
            return CreatedAtAction(nameof(ListarTodos), new { id = autorCreado.Id }, autorCreado);
        }
    }
}