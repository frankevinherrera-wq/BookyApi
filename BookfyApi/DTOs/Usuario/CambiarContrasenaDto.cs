
using System.ComponentModel.DataAnnotations;

namespace BookfyApi.DTOs.Usuario
{
    public class CambiarContrasenaDto
    {
        [Required]
        public string ContrasenaActual { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "La nueva contraseña debe tener al menos 6 caracteres.")]
        public string NuevaContrasena { get; set; } = string.Empty;
    }
}