using BookfyApi.DTOs.Usuario;

namespace BookfyApi.Services.Interfaces;

public interface IAuthService
{
    // 1. Registro tradicional con correo y contraseña
    Task<UsuarioReadDto> RegisterAsync(UsuarioCreateDto dto);

    // 2. Login tradicional que devuelve el Token JWT
    Task<string> LoginAsync(UsuarioLoginDto dto);  

    // 3. Login / Registro con Google OAuth (recibe el idToken y devuelve JWT)
    Task<string> GoogleLoginAsync(GoogleLoginDto dto);

    // 4. Cambio de contraseña validando la contraseña actual
    Task<bool> CambiarContrasenaAsync(int usuarioId, CambiarContrasenaDto dto);
}