

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookfyApi.Models
{
    public class DetallePedido
    {
        public int Id { get; set; }

        [Required]
        public int PedidoId { get; set; }
        [ForeignKey("PedidoId")]
        public Pedido? Pedido { get; set; }

        [Required]
        public int LibroId { get; set; }
        [ForeignKey("LibroId")]
        public Libro? Libro { get; set; }

        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}