using BookfyApi.DTOs.DetallesPedidos;
namespace BookfyApi.DTOs.Pedido
{
    public class PedidoReadDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; } = string.Empty;
        public DateTime FechaPedido { get; set; }
        public decimal Total { get; set; }

        public ICollection<DetallePedidoReadDto> DetallesPedidos { get; set; } = new List<DetallePedidoReadDto>();
    }
}