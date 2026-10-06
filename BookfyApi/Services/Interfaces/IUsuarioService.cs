


using BookfyApi.DTOs.Usuario;

namespace BookfyApi.Services.Interfaces
{
    public interface IUsuarioService
    {

        Task<IEnumerable<UsuarioReadDto>> GetAllAsync();
        Task<UsuarioReadDto?> GetByIdAsync(int id);

        Task<UsuarioReadDto?> GetByEmailAsync(string email);

        Task<UsuarioReadDto> CreateAsync(UsuarioCreateDto dto);
        Task<bool> UpdateAsync(int id, UsuarioUpdateDto dto);
    }
}