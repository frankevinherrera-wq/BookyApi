using System.Security.Claims;
using BookfyApi.DTOs.Usuario;
using BookfyApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookfyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize] // Comentado temporalmente hasta configurar JWT en Program.cs
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // GET: api/usuarios (Solo Administrador)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UsuarioReadDto>>> GetAll()
        {
            var usuarios = await _usuarioService.GetAllAsync();
            return Ok(usuarios);
        }

        // GET: api/usuarios/perfil (Obtener perfil del usuario autenticado)
        [HttpGet("perfil")]
        [Authorize]
        public async Task<ActionResult<UsuarioReadDto>> GetPerfil()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int usuarioId))
            {
                return Unauthorized(new { mensaje = "Token inválido o no proporcionado." });
            }

            var usuario = await _usuarioService.GetByIdAsync(usuarioId);
            if (usuario == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado." });
            }

            return Ok(usuario);
        }

        // GET: api/usuarios/5 (Consultar usuario por ID)
        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult<UsuarioReadDto>> GetById(int id)
        {
            // Protección IDOR: Solo el Admin o el propio usuario pueden consultar
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            bool esAdmin = User.IsInRole("Admin");

            if (!esAdmin && userIdClaim != id.ToString())
            {
                return Forbid(); // HTTP 403 Forbidden
            }

            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null)
            {
                return NotFound(new { mensaje = $"No existe el usuario con ID {id}." });
            }

            return Ok(usuario);
        }

        // GET: api/usuarios/buscar?email=ejemplo@correo.com (Solo Administrador)
        [HttpGet("buscar")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UsuarioReadDto>> GetByEmail([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new { mensaje = "Debe proporcionar un correo electrónico para la búsqueda." });
            }

            var usuario = await _usuarioService.GetByEmailAsync(email);
            if (usuario == null)
            {
                return NotFound(new { mensaje = $"No se encontró ningún usuario registrado con el correo {email}." });
            }

            return Ok(usuario);
        }

        // POST: api/usuarios (Registro público)
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UsuarioReadDto>> Create([FromBody] UsuarioCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var nuevoUsuario = await _usuarioService.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = nuevoUsuario.Id },
                    nuevoUsuario
                );
            }
            catch (InvalidOperationException ex)
            {
                // Captura la excepción cuando el email ya está en uso
                return Conflict(new { mensaje = ex.Message }); // HTTP 409 Conflict
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al registrar el usuario.", detalle = ex.Message });
            }
        }

        // PUT: api/usuarios/5 (Actualizar datos de perfil)
        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UsuarioUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Protección IDOR: Solo el Admin o el propio usuario pueden modificar
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            bool esAdmin = User.IsInRole("Admin");

            if (!esAdmin && userIdClaim != id.ToString())
            {
                return Forbid();
            }

            try
            {
                var actualizado = await _usuarioService.UpdateAsync(id, dto);
                if (!actualizado)
                {
                    return NotFound(new { mensaje = $"No existe el usuario con ID {id}." });
                }

                return NoContent(); // HTTP 204
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar el usuario.", detalle = ex.Message });
            }
        }
    }
}