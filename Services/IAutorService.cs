using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookfyApi.DTOs;


namespace BookfyApi.Services
{
    public interface IAutorService
    {
        Task<List<AutorReadDto>> ListarTodos();
        Task<AutorReadDto> Crear(AutorCreateDto autor);
    }
}