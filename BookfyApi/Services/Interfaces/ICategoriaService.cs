


using BookfyApi.DTOs.Categoria;

namespace BookfyApi.Services.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaReadDto >> GetAllAsync();
        Task<CategoriaReadDto?> GetByIdAsync(int id);

        Task<CategoriaReadDto> CreateAsync(CategoriaCreateDto dto);
        Task<bool> UpdateAsync(int id, CategoriaUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}