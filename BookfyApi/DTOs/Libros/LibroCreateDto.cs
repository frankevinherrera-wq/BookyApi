using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookfyApi.DTOs.Libros
{
    public class LibroCreateDto
    {
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(150, ErrorMessage = "El título no puede superar los 150 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [Range(1000, 2100, ErrorMessage = "Ingrese un año de publicación válido.")]
        public int AnioPublicacion { get; set; }

        [Required(ErrorMessage = "Debe especificar el precio")]
        public decimal Precio {get;set;}

        [Required(ErrorMessage = "Debe especificar un autor.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un AutorId válido.")]
        public int AutorId { get; set; }

        [Required(ErrorMessage = "Debe especificar una categoría.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un CategoriaId válido.")]
        public int CategoriaId { get; set; }
    }
}


