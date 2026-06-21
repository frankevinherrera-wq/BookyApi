using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookfyApi.models
{
    public class Autor
    {

        [Key]
        public int Id { get; set; }
        [Required (ErrorMessage = "El nombre del autor es necesario")]
        [StringLength(20)]
        public string Nombre { get; set; } = string.Empty;
        public string Nacionalidad { get; set; } = string.Empty;

        public List<Libro> Libros { get; set; } = new();
    }
}