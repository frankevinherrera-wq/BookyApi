using BookfyApi.Data;
using BookfyApi.DTOs.Pedido;
using BookfyApi.DTOs.Usuario;
using BookfyApi.Models;
using BookfyApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


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

    
}