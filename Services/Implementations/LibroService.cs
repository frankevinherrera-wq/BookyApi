using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BookfyApi.Models;
using BookfyApi.Services.Interfaces;
using BookfyApi.Data;
using BookfyApi.DTOs.Libros;

namespace BookfyApi.Services.Implementations
{
    public class LibroService : ILibroService
    {
        private readonly ApplicationDbContext _context;
        public LibroService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<LibroReadDto>> GetAllAsync()
        {
            return await _context.Libros
            .AsNoTracking() 
            .Select(l => new LibroReadDto
            {
                Id = l.Id,
                Titulo = l.Titulo,
                AnioPublicacion = l.AnioPublicacion,


                AutorId = l.AutorId,
                AutorNombre = l.Autor!.Nombre,

                CategoriaId = l.CategoriaId,
                CategoriaNombre = l.Categoria!.Nombre
            }).ToListAsync();
        } 

        public async Task<LibroReadDto?> GetByIdAsync(int id)
        {
            return await _context.Libros
            .AsNoTracking()
            .Where(l => l.Id == id)
            .Select(l => new LibroReadDto
            {
                Id = l.Id,
                Titulo = l.Titulo,
                AnioPublicacion = l.AnioPublicacion,
                AutorId = l.AutorId,
                AutorNombre = l.Autor!.Nombre,
                CategoriaId = l.CategoriaId,
                CategoriaNombre = l.Categoria!.Nombre
            })
            .FirstOrDefaultAsync();
        }


        public async Task<LibroReadDto> CreateAsync(LibroCreateDto dto)
        {

            var autor = await _context.Autores.FindAsync(dto.AutorId)
            ?? throw new ArgumentException($"No se encontró un autor con el ID {dto.AutorId}.");

            var categoria = await _context.Categorias.FindAsync(dto.CategoriaId)
            ?? throw new ArgumentException($"No se encontró una categoría con el ID {dto.CategoriaId}.");
            
            
            var nuevoLibro = new Libro
            {
                Titulo = dto.Titulo,
                AnioPublicacion = dto.AnioPublicacion,
                AutorId = dto.AutorId,
                CategoriaId = dto.CategoriaId
            };
            
            _context.Libros.Add(nuevoLibro);
            await _context.SaveChangesAsync();

            var libroDto = new LibroReadDto
            {
                Id = nuevoLibro.Id,
                Titulo = nuevoLibro.Titulo,
                AnioPublicacion = nuevoLibro.AnioPublicacion,
                AutorId = nuevoLibro.AutorId,
                AutorNombre = autor.Nombre,
                CategoriaId = nuevoLibro.CategoriaId,
                CategoriaNombre = categoria.Nombre
            };

            return libroDto;
        }
        public async Task<bool> UpdateAsync(int id, LibroUpdateDto update)
        {
            
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null) return false;

            if (!await _context.Autores.AnyAsync(a => a.Id == update.AutorId))
            {
                throw new ArgumentException($"No se encontró un autor con el ID {update.AutorId}.");
            }

            if (!await _context.Categorias.AnyAsync(c => c.Id == update.CategoriaId))
            {
                throw new ArgumentException($"No se encontró una categoría con el ID {update.CategoriaId}.");
            }

            libro.Titulo = update.Titulo;
            libro.AnioPublicacion = update.AnioPublicacion;
            libro.AutorId = update.AutorId;
            libro.CategoriaId = update.CategoriaId;


            await _context.SaveChangesAsync();

            return true;

            
        }

        public async Task<bool> DeleteAsync (int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null) return false;

            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();

            return true;
        
        }


        public async Task<IEnumerable<LibroReadDto>> GetByCategoriaAsync(int categoriaId)        {
            return await _context.Libros
            .AsNoTracking()
            .Where(l => l.CategoriaId == categoriaId) 
            .Select(l => new LibroReadDto
            {
                Id = l.Id,
                Titulo = l.Titulo,
                AnioPublicacion = l.AnioPublicacion,
                AutorId = l.AutorId,
                AutorNombre = l.Autor!.Nombre,
                CategoriaId = l.CategoriaId,
                CategoriaNombre = l.Categoria!.Nombre
            })
            .ToListAsync();

        }

        public async Task<IEnumerable<LibroReadDto>> GetByAutorAsync(int autorId)        {
            return await _context.Libros
            .AsNoTracking()
            .Where(l => l.AutorId == autorId) 
            .Select(l => new LibroReadDto
            {
                Id = l.Id,
                Titulo = l.Titulo,
                AnioPublicacion = l.AnioPublicacion,
                AutorId = l.AutorId,
                AutorNombre = l.Autor!.Nombre,
                CategoriaId = l.CategoriaId,
                CategoriaNombre = l.Categoria!.Nombre
            })
            .ToListAsync();

        }

         public async Task<IEnumerable<LibroReadDto>> SearchByTituloAsync(string titulo)        {
            return await _context.Libros
            .AsNoTracking()
            .Where(l => l.Titulo.Contains(titulo)) 
            .Select(l => new LibroReadDto
            {
                Id = l.Id,
                Titulo = l.Titulo,
                AnioPublicacion = l.AnioPublicacion,
                AutorId = l.AutorId,
                AutorNombre = l.Autor!.Nombre,
                CategoriaId = l.CategoriaId,
                CategoriaNombre = l.Categoria!.Nombre
            })
            .ToListAsync();

        }



    }
}