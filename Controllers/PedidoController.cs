using BookfyApi.DTOs.Pedido;
using BookfyApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
namespace BookfyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        // GET: api/pedidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoReadDto>>> GetAll()
        {
            var pedidos = await _pedidoService.GetAllAsync();
            return Ok(pedidos);
        }

        // GET: api/pedidos/5
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

        // GET: api/pedidos/usuario/3
        [HttpGet("usuario/{usuarioId:int}")]
        public async Task<ActionResult<IEnumerable<PedidoReadDto>>> GetByUsuarioId(int usuarioId)
        {
            var pedidos = await _pedidoService.GetByUsuarioIdAsync(usuarioId);
            return Ok(pedidos);
        }

        // POST: api/pedidos
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

                // Devuelve HTTP 201 Created con el encabezado Location -> GET api/pedidos/{id}
                return CreatedAtAction(
                    nameof(GetById), 
                    new { id = nuevoPedido.Id }, 
                    nuevoPedido
                );
            }
            catch (KeyNotFoundException ex)
            {
                // En caso de que un LibroId enviado no exista en la base de datos
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error al procesar el pedido.", detalle = ex.Message });
            }
        }

        // PATCH: api/pedidos/5/estado
        [HttpPatch("{id:int}/estado")]
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

            return NoContent(); // HTTP 204: Cambio exitoso sin contenido que devolver
        }
    }
}