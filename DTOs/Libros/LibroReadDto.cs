
namespace BookfyApi.DTOs.Libros;

public class LibroReadDto
{

    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int AnioPublicacion { get; set; }



    public int AutorId { get; set; }
    public string AutorNombre { get; set; } = string.Empty;

    
    public int CategoriaId { get; set; }
    public string CategoriaNombre { get; set; } = string.Empty;

}
