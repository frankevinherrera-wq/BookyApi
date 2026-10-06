

using Microsoft.EntityFrameworkCore;
using BookfyApi.Data;
using BookfyApi.Models;
using BookfyApi.DTOs.Autor;
using BookfyApi.Services.Interfaces;



namespace BookfyApi.Services.Implementations;

public class AutorService : IAutorService
{
    private readonly ApplicationDbContext _context;

    public AutorService(ApplicationDbContext context)
    {
        _context = context;
    }



    public async Task<IEnumerable<AutorReadDto>> GetAllAsync()
    {
        return await _context.Autores
        .Select(a => new AutorReadDto
        {
            Id = a.Id,
            Nombre = a.Nombre,
            Nacionalidad = a.Nacionalidad
        })
        .ToListAsync();
    }


    public async Task<AutorReadDto?> GetByIdAsync(int id)
    {
        var busq = await _context.Autores.FindAsync(id);
        if (busq == null) return null;

        var dto = new AutorReadDto
        {
            Id = busq.Id,
            Nombre = busq.Nombre,
            Nacionalidad = busq.Nacionalidad
        };
        return dto;

    }


    public async Task<AutorReadDto> CreateAsync(AutorCreateDto dto)
    {
        var nuevo = new Autor
        {
            Nombre = dto.Nombre,
            Nacionalidad = dto.Nacionalidad
        };

        _context.Autores.Add(nuevo);
        await _context.SaveChangesAsync();

        var dtoRequest = new AutorReadDto
        {
            Id = nuevo.Id,
            Nombre = nuevo.Nombre,
            Nacionalidad = nuevo.Nacionalidad
        };

        return dtoRequest;

        
    }

    public async Task<bool> UpdateAsync(int id,  AutorUpdateDto dto)
    {
        var busq = await _context.Autores.FindAsync(id);
        if (busq == null) return false;

        busq.Nombre = dto.Nombre;
        busq.Nacionalidad = dto.Nacionalidad;

        await _context.SaveChangesAsync();
        return true;
    
    
    }




    public async Task<bool> DeleteAsync(int id)
    {
        var busq = await _context.Autores.FindAsync(id);

        if (busq == null) return false;

        _context.Autores.Remove(busq);
        await _context.SaveChangesAsync();
        return true;
    }


}

