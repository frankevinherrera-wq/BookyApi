using System.ComponentModel.DataAnnotations;

namespace BookfyApi.DTOs.DetallePedido
{
    public class DetallePedidoCreateDto
    {
        [Required(ErrorMessage = "Debe especificar un libro.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un LibroId válido.")]
        public int LibroId { get; set; }

        [Range(1, 100, ErrorMessage = "La cantidad debe ser de al menos 1 unidad.")]
        public int Cantidad { get; set; }
    }
}