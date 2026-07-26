using BookfyApi.DTOs.DetallePedido;

namespace BookfyApi.DTOs.Pedido
{
    public class PedidoReadDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; } = string.Empty;
        public DateTime FechaPedido { get; set; }
        public decimal Total { get; set; }

        // Incluimos los detalles mapeados con su respectivo DTO
        public List<DetallePedidoReadDto> Detalles { get; set; } = new();
    }
}