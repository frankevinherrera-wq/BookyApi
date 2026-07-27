using BookfyApi.DTOs.Libros;



namespace BookfyApi.Services.Interfaces;

public interface ILibroService
{
    Task<IEnumerable<LibroReadDto>> GetAllAsync();
    Task<LibroReadDto?> GetByIdAsync(int id);

    Task<LibroReadDto> CreateAsync(LibroCreateDto dto);
    Task<bool> UpdateAsync(int id, LibroUpdateDto dto);
    Task<bool> DeleteAsync(int id);



     Task<IEnumerable<LibroReadDto>> GetByCategoriaAsync(int categoriaId); 
    Task<IEnumerable<LibroReadDto>> GetByAutorAsync(int autorId);         
    Task<IEnumerable<LibroReadDto>> SearchByTituloAsync(string titulo);  
}