using System;


namespace BookfyApi.Models
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int AnioPublicacion { get; set; }
        

        public int AutorId { get; set; }
        public Autor? Autor { get; set; }


        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }    
        public ICollection<DetallePedido>? DetallesPedidos { get; set; } =new List<DetallePedido>();

    }
}