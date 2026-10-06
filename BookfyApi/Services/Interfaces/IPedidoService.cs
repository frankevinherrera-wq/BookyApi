


using BookfyApi.DTOs.Pedido;

namespace BookfyApi.Services.Interfaces
{
    public interface IPedidoService
    {


        Task<IEnumerable<PedidoReadDto>> GetByUsuarioIdAsync(int usuarioId);



        Task<IEnumerable<PedidoReadDto >> GetAllAsync();
        Task<PedidoReadDto?> GetByIdAsync(int id);

        Task<PedidoReadDto> CreateAsync(PedidoCreateDto dto);

        Task<bool> UpdateEstadoAsync(int id, string nuevoEstado);
    }
}