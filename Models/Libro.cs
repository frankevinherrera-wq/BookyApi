using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookfyApi.Models
{
    public class Libro
    {
        [Key]
        public int Id { get; set; }

        [Required (ErrorMessage = "El título del libro es necesario")]
        [StringLength(100)]
        public string Titulo { get; set; } = string.Empty;
        

        [Range (1900, 2026, ErrorMessage = "El año de publicación debe estar entre 1900 y 2026")]
        public int AnioPublicacion { get; set; }
        
        [Required]
        public int AutorId { get; set; }

        [ForeignKey("AutorId")]
        public Autor? Autor { get; set; }
    }
}