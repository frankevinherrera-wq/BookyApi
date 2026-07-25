using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BookfyApi.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public DateTime FechaPedido { get; set; }
        public decimal Total { get; set; }
        public ICollection<DetallePedido>? DetallesPedidos { get; set; }

    }
}