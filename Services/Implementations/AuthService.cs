using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Google.Apis.Auth;
using BookfyApi.Data;
using BookfyApi.Models;
using BookfyApi.DTOs.Usuario;
using BookfyApi.Services.Interfaces;


namespace BookfyApi.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config; // 1. Inyectamos IConfiguration para leer appsettings.json

    public AuthService(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<UsuarioReadDto> RegisterAsync(UsuarioCreateDto dto)
    {
        bool emailExiste = await _context.Usuarios
            .AnyAsync(u => u.Email.ToLower() == dto.Email.ToLower());

        if (emailExiste)
        {
            throw new InvalidOperationException("El correo electrónico ya está registrado.");
        }

        // Hashear la contraseña con BCrypt
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena);

        var nuevoUsuario = new Usuario
        {
            Nombre = dto.Nombre,
            Email = dto.Email,
            Contrasena = passwordHash,
            FechaRegistro = DateTime.UtcNow,
        };

        _context.Usuarios.Add(nuevoUsuario);
        await _context.SaveChangesAsync();

        return new UsuarioReadDto
        {
            Id = nuevoUsuario.Id,
            Nombre = nuevoUsuario.Nombre,
            Email = nuevoUsuario.Email,
            FechaRegistro = nuevoUsuario.FechaRegistro
        };
    }
    public async Task<string> LoginAsync(UsuarioLoginDto dto)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email.ToLower());

        // Validar existencia y contraseña
        if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Contrasena, usuario.Contrasena))
        {
            throw new InvalidOperationException("Credenciales inválidas.");
        }



        // Generar y retornar el token JWT
        return GenerarJwtToken(usuario);
    }
    // 3. LOGIN CON GOOGLE
    public async Task<string> GoogleLoginAsync(GoogleLoginDto dto)
    {
        GoogleJsonWebSignature.Payload payload;

        try
        {
            // Validar el token enviado desde el Frontend con la librería oficial de Google
            payload = await GoogleJsonWebSignature.ValidateAsync(dto.IdToken);
        }
        catch (Exception)
        {
            throw new InvalidOperationException("El token de Google no es válido.");
        }

        // Buscar al usuario por Email
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email.ToLower() == payload.Email.ToLower());


        // Si no existe, lo creamos automáticamente
        if (usuario == null)
        {
            usuario = new Usuario
            {
                Nombre = payload.Name,
                Email = payload.Email,
                Contrasena = "", // No requiere clave local porque ingresa vía Google
                FechaRegistro = DateTime.UtcNow,
           };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        // Emitir nuestro propio JWT
        return GenerarJwtToken(usuario);
    }
    public async Task<bool> CambiarContrasenaAsync(int usuarioId, CambiarContrasenaDto dto)
    {
        // 1. Buscar el usuario por su ID
        var usuario = await _context.Usuarios.FindAsync(usuarioId);

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

        // 5. Hashear e ingresar la nueva contraseña
        usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.NuevaContrasena);

        // 6. Guardar los cambios en la BD
        await _context.SaveChangesAsync();

        return true;
    }

    private string GenerarJwtToken(Usuario usuario)
    {
        // Definir los Claims (Pares Clave-Valor en el Carnet Digital)
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Name, usuario.Nombre),
        };

        var secretKey = _config["Jwt:SecretKey"] 
            ?? throw new InvalidOperationException("No se ha configurado la clave secreta Jwt:SecretKey.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Crear la estructura del token
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8), // Duración de la sesión
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}