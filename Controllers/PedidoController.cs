using BookfyApi.DTOs.Pedido;
using BookfyApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookfyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize] // Nota: Si aún no has configurado JWT en Program.cs, puedes dejarlo comentado para probar libremente
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        // GET: api/pedidos (Solo Administrador)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<PedidoReadDto>>> GetAll()
        {
            var pedidos = await _pedidoService.GetAllAsync();
            return Ok(pedidos);
        }

        // GET: api/pedidos/5 (Obtener un pedido por su ID de Pedido)
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PedidoReadDto>> GetById(int id)
        {
            var pedido = await _pedidoService.GetByIdAsync(id);

            if (pedido == null)
            {
                return NotFound(new { mensaje = $"No se encontró el pedido con ID {id}." });
            }

            return Ok(pedido);
        }

        // GET: api/pedidos/usuario/3 (Obtener todos los pedidos de un Usuario)
        [HttpGet("usuario/{usuarioId:int}")]
        public async Task<ActionResult<IEnumerable<PedidoReadDto>>> GetByUsuarioId(int usuarioId)
        {
            var pedidos = await _pedidoService.GetByUsuarioIdAsync(usuarioId);
            return Ok(pedidos);
        }

        // POST: api/pedidos (Crear un pedido)
        [HttpPost]
        public async Task<ActionResult<PedidoReadDto>> Create([FromBody] PedidoCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var nuevoPedido = await _pedidoService.CreateAsync(dto);

                // Devuelve HTTP 201 Created apontando a GET api/pedidos/{id}
                return CreatedAtAction(
                    nameof(GetById), 
                    new { id = nuevoPedido.Id }, 
                    nuevoPedido
                );
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al procesar el pedido.", detalle = ex.Message });
            }
        }

        // PATCH: api/pedidos/5/estado (Solo Administrador)
        [HttpPatch("{id:int}/estado")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEstado(int id, [FromBody] PedidoUpdateEstadoAdminDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var actualizado = await _pedidoService.UpdateEstadoAsync(id, dto.NuevoEstado);

            if (!actualizado)
            {
                return NotFound(new { mensaje = $"No se pudo actualizar. No existe el pedido con ID {id}." });
            }

            return NoContent();
        }
    }
}