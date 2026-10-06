using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using BookfyApi.Data;
using BookfyApi.DTOs.Usuario;
using BookfyApi.Models;
using BookfyApi.Services.Interfaces;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
namespace BookfyApi.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config; //permite leer los valores del archivo appsetting json, variable de entorno o user secret
    private readonly ILogger<AuthService> _logger;


    private const string DummyHash =
        "$2a$11$CqZ8jH8rV9G1kQmM8yQyMeQpV3qz1zqjv0m8Jn9K1ZpQGZP8yqe8O"; //para igualar tiempo de respuesta

    public AuthService(ApplicationDbContext context, IConfiguration config, ILogger<AuthService> logger)
    {
        _context = context;
        _config = config;
        _logger = logger;
    }

    public async Task<UsuarioReadDto> RegisterAsync(UsuarioCreateDto dto, CancellationToken ct = default ) //Cancelación de tareas asíncronas.
    {
        bool emailExiste = await _context.Usuarios
            .AnyAsync(u => u.Email.ToLower() == dto.Email.ToLower(), ct);

        if (emailExiste)
        {
            throw new InvalidOperationException("El correo electrónico ya está registrado.");
        }

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena);

        var nuevoUsuario = new Usuario
        {
            Nombre = dto.Nombre,
            Email = dto.Email,
            Contrasena = passwordHash,
            FechaRegistro = DateTime.UtcNow,
            Rol = "Cliente" // Asignación explícita por seguridad
        };

        _context.Usuarios.Add(nuevoUsuario);
        await _context.SaveChangesAsync(ct);

        return new UsuarioReadDto
        {
            Id = nuevoUsuario.Id,
            Nombre = nuevoUsuario.Nombre,
            Email = nuevoUsuario.Email,
            FechaRegistro = nuevoUsuario.FechaRegistro,
        };
    }

    public async Task<string> LoginAsync(UsuarioLoginDto dto, CancellationToken ct = default)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email.ToLower(), ct);

        // Si el usuario no existe, igual corremos un Verify contra un hash dummy
        // para que el tiempo de respuesta no delate si el email está registrado.
        bool contrasenaValida;
        if (usuario == null || string.IsNullOrEmpty(usuario.Contrasena))
        {
            BCrypt.Net.BCrypt.Verify(dto.Contrasena, DummyHash);
            contrasenaValida = false;
        }
        else
        {
            contrasenaValida = BCrypt.Net.BCrypt.Verify(dto.Contrasena, usuario.Contrasena);
        }

        if (usuario == null || !contrasenaValida)
        {
            throw new InvalidOperationException("Credenciales inválidas.");
        }

        return GenerarJwtToken(usuario);//arquitectura sin estado (Stateless).
        //el JWT solo se genera cuando el usuario demuestra su identidad.
    }

    public async Task<string> GoogleLoginAsync(GoogleLoginDto dto, CancellationToken ct = default)
    {
        GoogleJsonWebSignature.Payload payload; // payload donde se guardarán los datos del perfil del usuario

        var googleClientId = _config["Google:ClientId"] 
            ?? throw new InvalidOperationException("No se ha configurado Google:ClientId.");

        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { googleClientId }
            };

            payload = await GoogleJsonWebSignature.ValidateAsync(dto.IdToken, settings);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Fallo al validar el token de Google.");
            throw new InvalidOperationException("El token de Google no es válido.");
        }

        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email.ToLower() == payload.Email.ToLower(), ct);

        if (usuario == null)
        {
            usuario = new Usuario
            {
                Nombre = payload.Name,
                Email = payload.Email,
                Contrasena = "", // Sin contraseña local por ser cuenta OAuth
                FechaRegistro = DateTime.UtcNow,
                Rol = "Cliente"
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync(ct);
        }

        return GenerarJwtToken(usuario);
    }

    public async Task<bool> CambiarContrasenaAsync(int usuarioId, CambiarContrasenaDto dto, CancellationToken ct = default)
    {
        var usuario = await _context.Usuarios.FindAsync(new object[] { usuarioId }, ct);

        if (usuario == null)
        {
            throw new InvalidOperationException("Usuario no encontrado.");
        }

        if (string.IsNullOrEmpty(usuario.Contrasena))
        {
            throw new InvalidOperationException("Esta cuenta inició sesión con Google y no posee una contraseña local asignada.");
        }

        bool contrasenaCorrecta = BCrypt.Net.BCrypt.Verify(dto.ContrasenaActual, usuario.Contrasena);
        if (!contrasenaCorrecta)
        {
            throw new InvalidOperationException("La contraseña actual es incorrecta.");
        }

        if (dto.ContrasenaActual == dto.NuevaContrasena)
        {
            throw new InvalidOperationException("La nueva contraseña debe ser diferente a la contraseña actual.");
        }

        usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.NuevaContrasena);
        await _context.SaveChangesAsync(ct);

        return true;
    }

    private string GenerarJwtToken(Usuario usuario)
    {
        // Claims clave para autorización y contexto de sesión
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(ClaimTypes.Role, string.IsNullOrWhiteSpace(usuario.Rol) ? "Cliente" : usuario.Rol) // Esencial para [Authorize(Roles = "...")]
        };

        var secretKey = _config["Jwt:SecretKey"]
            ?? throw new InvalidOperationException("No se ha configurado la clave secreta Jwt:SecretKey.");

        if (Encoding.UTF8.GetByteCount(secretKey) < 32)
        {
            throw new InvalidOperationException(
                "Jwt:SecretKey debe tener al menos 32 caracteres (256 bits) para HmacSha256.");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}