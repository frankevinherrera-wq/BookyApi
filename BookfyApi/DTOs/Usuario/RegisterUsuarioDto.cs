using System.ComponentModel.DataAnnotations;


namespace BookfyApi.DTOs;

public class RegisterUsuarioDto
{
    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
    public string Contrasena { get; set; } = string.Empty;
}