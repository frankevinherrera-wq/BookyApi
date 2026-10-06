using BookfyApi.DTOs.Usuario;

namespace BookfyApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UsuarioReadDto> RegisterAsync(UsuarioCreateDto dto, CancellationToken ct = default);
        
        // 👈 Asegúrate de que tenga ', CancellationToken ct = default'
        Task<string> LoginAsync(UsuarioLoginDto dto, CancellationToken ct = default);
        
        Task<string> GoogleLoginAsync(GoogleLoginDto dto, CancellationToken ct = default);
        
        Task<bool> CambiarContrasenaAsync(int usuarioId, CambiarContrasenaDto dto, CancellationToken ct = default);
    }
}