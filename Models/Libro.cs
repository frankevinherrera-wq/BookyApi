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

        public string Titulo { get; set; } = string.Empty;
        

        public int AnioPublicacion { get; set; }
        
        [Required]
        public int AutorId { get; set; }
        [ForeignKey("AutorId")]
        public Autor? Autor { get; set; }


        [Required]
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria? Categoria { get; set; }    
        public ICollection<DetallePedido>? DetallesPedidos { get; set; }

    }
}