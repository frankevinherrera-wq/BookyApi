using System.ComponentModel.DataAnnotations;

namespace BookfyApi.DTOs.Pedido
{
    public class PedidoUpdateEstadoAdminDto
    {
        [Required(ErrorMessage = "El estado es obligatorio.")]
        public string NuevoEstado { get; set; } = string.Empty;
    }
}