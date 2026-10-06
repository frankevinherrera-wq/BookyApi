


namespace BookfyApi.DTOs.DetallesPedidos{
    public class DetallePedidoReadDto
    {
        public int Id { get; set; }
        public int LibroId { get; set; }
        public string LibroTitulo { get; set; } = string.Empty;
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal => PrecioUnitario * Cantidad;
    }
}