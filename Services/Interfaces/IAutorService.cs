


using BookfyApi.DTOs.Autor;

namespace BookfyApi.Services.Interfaces
{
    public interface IAutorService
    {
        Task<IEnumerable<AutorReadDto>> GetAllAsync();
        Task<AutorReadDto?> GetByIdAsync(int id);

        Task<AutorReadDto> CreateAsync(AutorCreateDto dto);
        Task<bool> UpdateAsync(int id, AutorUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}