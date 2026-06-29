// DTOs/LibroDto.cs
using System.ComponentModel.DataAnnotations;

namespace BookfyApi.DTOs
{



    public class LibroReadDto
    {

        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int AnioPublicacion { get; set; }
    
    }
}