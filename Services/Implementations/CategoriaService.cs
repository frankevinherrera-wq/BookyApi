using Microsoft.EntityFrameworkCore;
using BookfyApi.Data;
using BookfyApi.Models;
using BookfyApi.DTOs.Categoria;
using BookfyApi.Services.Interfaces;


namespace BookfyApi.Services.Implementations;


public class CategoriaService : ICategoriaService
{
    private readonly ApplicationDbContext _context;
    public CategoriaService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CategoriaReadDto>> GetAllAsync()
    {
        return await _context.Categorias
        .Select(c => new CategoriaReadDto
            {
                Id = c.Id,
                Nombre = c.Nombre

            }).
            ToListAsync();
    }

    public async Task<CategoriaReadDto?> GetByIdAsync (int id)
    {
        var categoriaId = await _context.Categorias.FindAsync(id);

        if (categoriaId == null) return null;

        var dto = new  CategoriaReadDto
        {
            Id = categoriaId.Id,
            Nombre = categoriaId.Nombre
        };
        return dto;


    }


    public async Task<CategoriaReadDto> CreateAsync (CategoriaCreateDto dto)
    {
        var nuevoMod = new Categoria
        {
            Nombre = dto.Nombre
        };

        _context.Categorias.Add(nuevoMod);
        await _context.SaveChangesAsync();

        var dtoRequest = new CategoriaReadDto
        {
            Id = nuevoMod.Id,
            Nombre = nuevoMod.Nombre
        };

        return dtoRequest;
    }


    public async Task<bool> UpdateAsync( int id, CategoriaUpdateDto update)
    {
        var busq = await _context.Categorias.FindAsync(id);
        if (busq == null) return false;

        busq.Nombre = update.Nombre;
        await _context.SaveChangesAsync();

        return true;

    }

    public async Task<bool> DeleteAsync (int id)
    {
        var del = await _context.Categorias.FindAsync(id);
        if (del== null) return false;

        _context.Categorias.Remove(del);
        await _context.SaveChangesAsync();
        return true;
    }

}