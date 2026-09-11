using System.ComponentModel.DataAnnotations;
using BookfyApi.DTOs.DetallesPedidos;
namespace BookfyApi.DTOs.Pedido
{
    public class PedidoCreateDto
    {
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El pedido debe contener al menos un producto.")]
        [MinLength(1, ErrorMessage = "Debe agregar al menos un ítem al pedido.")]
        public List<DetallePedidoCreateDto> Detalles { get; set; } = new();
    }
}