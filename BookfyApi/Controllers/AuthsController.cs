using System.Security.Claims;
using BookfyApi.DTOs.Usuario;
using BookfyApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookfyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registro de un nuevo usuario local.
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<UsuarioReadDto>> Register([FromBody] UsuarioCreateDto dto, CancellationToken ct)
    {
        try
        {
            var usuario = await _authService.RegisterAsync(dto, ct);
            return CreatedAtAction(nameof(Register), new { id = usuario.Id }, usuario);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Inicio de sesión con credenciales locales (devuelve el JWT).
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<object>> Login([FromBody] UsuarioLoginDto dto, CancellationToken ct)
    {
        try
        {
            var token = await _authService.LoginAsync(dto, ct);
            return Ok(new { token });
        }
        catch (InvalidOperationException ex)
        {
            // Devuelve 401 Unauthorized cuando las credenciales no son válidas
            return Unauthorized(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Inicio de sesión o registro vía Google OAuth (devuelve el JWT).
    /// </summary>
    [HttpPost("google-login")]
    public async Task<ActionResult<object>> GoogleLogin([FromBody] GoogleLoginDto dto, CancellationToken ct)
    {
        try
        {
            var token = await _authService.GoogleLoginAsync(dto, ct);
            return Ok(new { token });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Cambia la contraseña del usuario actualmente autenticado.
    /// </summary>
    [HttpPost("cambiar-contrasena")]
    [Authorize] // Requiere que la petición lleve el JWT válido en el header Authorization
    public async Task<IActionResult> CambiarContrasena([FromBody] CambiarContrasenaDto dto, CancellationToken ct)
    {
        // Se extrae el ID del usuario directamente desde el Claim del JWT de la petición
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int usuarioId))
        {
            return Unauthorized(new { mensaje = "Token inválido o claim de usuario no encontrado." });
        }

        try
        {
            await _authService.CambiarContrasenaAsync(usuarioId, dto, ct);
            return Ok(new { mensaje = "Contraseña actualizada exitosamente." });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
}