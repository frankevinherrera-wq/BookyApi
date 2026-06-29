using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookfyApi.DTOs
{
    public class AutorCreateDto
    {
        [Required(ErrorMessage = "El nombre del autor es necesario")]
        [StringLength(20)]
        public string Nombre { get; set; } = string.Empty;
        public string Nacionalidad { get; set; } = string.Empty;
    }
}