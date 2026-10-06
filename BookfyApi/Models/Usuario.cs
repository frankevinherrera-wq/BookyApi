using System;

namespace BookfyApi.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public string Rol { get; set; } = "Cliente";
        public ICollection<Pedido>? Pedidos { get; set; } = new List<Pedido>();
    }
}