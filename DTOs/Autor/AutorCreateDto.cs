using System.ComponentModel.DataAnnotations;

namespace BookfyApi.DTOs.Autor
{
    public class AutorCreateDto
    {
        [Required(ErrorMessage = "El nombre del autor es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La nacionalidad es obligatoria.")]
        [StringLength(50, ErrorMessage = "La nacionalidad no puede exceder los 50 caracteres.")]
        public string Nacionalidad { get; set; } = string.Empty;
    }
}