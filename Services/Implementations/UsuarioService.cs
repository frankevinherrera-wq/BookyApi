using BookfyApi.Data;
using BookfyApi.DTOs.Pedido;
using BookfyApi.DTOs.Usuario;
using BookfyApi.Models;
using BookfyApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace BookfyApi.Services;

public  class UsuarioService : IUsuarioService
{
    private readonly ApplicationDbContext _context;

    public UsuarioService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UsuarioReadDto>> GetAllAsync()
    {
        return await _context.Usuarios
                .AsNoTracking() //aumenta velocidad en consulta porque no rasrtrea para cambios 
                .Select(u => new UsuarioReadDto
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Email = u.Email,
                    FechaRegistro = u.FechaRegistro
                }).ToListAsync();

    }

    public async Task<UsuarioReadDto?> GetByIdAsync(int id)
    {
        return await _context.Usuarios
                .AsNoTracking()
                .Where(u => id == u.Id)
                .Select(u => new UsuarioReadDto{
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Email = u.Email,
                    FechaRegistro = u.FechaRegistro
                }).FirstOrDefaultAsync();// Retorna el DTO o null si no existe
    }


    public async Task<UsuarioReadDto?> GetByEmailAsync(string email)
    {
        return await _context.Usuarios
                .AsNoTracking()
                .Where(u => email == u.Email)
                .Select(u => new UsuarioReadDto{
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Email = u.Email,
                    FechaRegistro = u.FechaRegistro
                }).FirstOrDefaultAsync();
        

    }


    public async Task<UsuarioReadDto> CreateAsync(UsuarioCreateDto dto)
    {
        bool emailExiste = await _context.Usuarios
        .AnyAsync(u => u.Email.ToLower() == dto.Email.ToLower());

        if (emailExiste)
        {
   
            throw new InvalidOperationException("El correo electrónico ya está en uso.");
        }

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena);

        var newUser = new Usuario
        {
            Nombre = dto.Nombre,
            Email = dto.Email, 
            Contrasena = passwordHash,
            FechaRegistro = DateTime.UtcNow

        };

        _context.Usuarios.Add(newUser);
        await _context.SaveChangesAsync();


        return new UsuarioReadDto
        {
            Id = newUser.Id,
            Nombre = newUser.Nombre,
            Email = newUser.Email, 
            FechaRegistro = newUser.FechaRegistro
        };
    }


    public async Task<bool> UpdateAsync(int id, UsuarioUpdateDto dto)
    {
        
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return false;

        usuario.Nombre = dto.Nombre;
        usuario.Email = dto.Email;

        await _context.SaveChangesAsync();
        return true;
    
    }




    
}