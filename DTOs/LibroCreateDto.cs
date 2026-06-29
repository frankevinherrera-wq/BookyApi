using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookfyApi.DTOs
{
    public class LibroCreateDto
    {
        [Required (ErrorMessage = "El título del libro es necesario")]
        [StringLength(100, ErrorMessage = "El título no puede superar los 100 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [Range(1900, 2026, ErrorMessage = "El año de publicación debe estar entre 1900 y 2026")]
        public int AnioPublicacion { get; set; }

        [Required(ErrorMessage = "El ID del autor es requerido")]
        public int? AutorId { get; set; }
    }
}


