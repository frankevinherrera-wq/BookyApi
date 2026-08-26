

using BookfyApi.DTOs.Usuario;


namespace BookfyApi.Services.Interfaces;
public interface IAuthService
{
    Task<UsuarioReadDto> RegisterAsync(UsuarioCreateDto dto);
    Task<string> LoginAsync(UsuarioLoginDto dto);  
}