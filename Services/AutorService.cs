using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookfyApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using BookfyApi.Data;
using BookfyApi.Models;

namespace BookfyApi.Services
{
    public class AutorService: IAutorService
    {
        private readonly ApplicationDbContext  _context;

        public AutorService(ApplicationDbContext context)
        {
            _context = context;
        }
         public async Task<List<AutorReadDto>> ListarTodos()
        {
            var autores = await _context.Autores.ToListAsync();
            
            var autoresDto = autores.Select(a => new AutorReadDto 
            { 
                Id = a.Id, 
                Nombre = a.Nombre, 
                Nacionalidad = a.Nacionalidad 
            }).ToList();
            return autoresDto;
        }

        public async Task<AutorReadDto> Crear(AutorCreateDto autorDto)
        {
            var autor = new Autor
            {
                Nombre = autorDto.Nombre,
                Nacionalidad = autorDto.Nacionalidad
            };

            _context.Autores.Add(autor);
            await _context.SaveChangesAsync();

            var autorReadDto = new AutorReadDto
            {
                Id = autor.Id,
                Nombre = autor.Nombre,
                Nacionalidad = autor.Nacionalidad
            };

            return autorReadDto;
        }
    }
}