using BookfyApi.Data;
using BookfyApi.DTOs.DetallesPedidos;
using BookfyApi.DTOs.Pedido;
using BookfyApi.Models;
using BookfyApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookfyApi.Services.Implementations
{
    public class PedidoService : IPedidoService
    {
        private readonly ApplicationDbContext _context;

        public PedidoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PedidoReadDto>> GetAllAsync()
        {
            return await _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.DetallesPedidos)
                .AsNoTracking()

                .Select(p => new PedidoReadDto
                {
                    Id = p.Id,
                    UsuarioId = p.UsuarioId,
                    UsuarioNombre = p.Usuario != null ? $"{p.Usuario.Nombre}".Trim() : string.Empty,
                    FechaPedido = p.FechaPedido,
                    Total = p.Total,
                    DetallesPedidos = (p.DetallesPedidos ?? new List<DetallePedido>()).Select(d => new DetallePedidoReadDto
                    {
                        Id = d.Id,
                        LibroId = d.LibroId,
                        LibroTitulo = d.Libro != null ? d.Libro.Titulo : string.Empty,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario,
                    }).ToList() 
                })
                .ToListAsync();
        }

        public async Task<PedidoReadDto?> GetByIdAsync(int id)
        {
            return await _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.DetallesPedidos)
                .AsNoTracking()

                .Where(p => p.Id == id)
                .Select(p => new PedidoReadDto
                {
                    Id = p.Id,
                    UsuarioId = p.UsuarioId,
                    UsuarioNombre = p.Usuario != null ? $"{p.Usuario.Nombre}".Trim() : string.Empty,
                    FechaPedido = p.FechaPedido,
                    Total = p.Total,
                    DetallesPedidos = (p.DetallesPedidos ?? new List<DetallePedido>()).Select(d => new DetallePedidoReadDto
                    {
                        Id = d.Id,
                        LibroId = d.LibroId,
                        LibroTitulo = d.Libro != null ? d.Libro.Titulo : string.Empty,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario,
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PedidoReadDto>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.DetallesPedidos)
                .Where(p => p.UsuarioId == usuarioId)
                .AsNoTracking()
                .Select(p => new PedidoReadDto
                {
                    Id = p.Id,
                    UsuarioId = p.UsuarioId,
                    UsuarioNombre = p.Usuario != null ? $"{p.Usuario.Nombre}".Trim() : string.Empty,
                    FechaPedido = p.FechaPedido,
                    Total = p.Total,
                    DetallesPedidos = (p.DetallesPedidos ?? new List<DetallePedido>()).Select(d => new DetallePedidoReadDto
                    {
                        Id = d.Id,
                        LibroId = d.LibroId,
                        LibroTitulo = d.Libro != null ? d.Libro.Titulo : string.Empty,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario,
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<PedidoReadDto> CreateAsync(PedidoCreateDto dto)
        {
            // 1. Extraemos los LibroId que envió el usuario
            var libroIds = dto.Detalles.Select(d => d.LibroId).Distinct().ToList();

            var libroBD = await _context.Libros
                        .Where(l => libroIds.Contains(l.Id))
                        .ToDictionaryAsync(l => l.Id, l => l.Precio);
            


            // 1. Instanciamos el Modelo calculando el Total directamente desde 'dto.Detalles'
            var nuevoPedido = new Pedido
            {
                UsuarioId = dto.UsuarioId,
                FechaPedido = DateTime.UtcNow,


                DetallesPedidos = dto.Detalles.Select(d => new DetallePedido
                {
                    LibroId = d.LibroId,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = libroBD[d.LibroId]
                }).ToList()
            };

            // 2. Guardamos en la base de datos
            _context.Pedidos.Add(nuevoPedido);
            await _context.SaveChangesAsync();



            // 3. Consultamos el pedido recién guardado incluyendo Usuario y Libro
            var pedidoGuardado = await _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.DetallesPedidos!)
                    .ThenInclude(d => d.Libro)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == nuevoPedido.Id);

            if (pedidoGuardado == null)
            {
                throw new Exception("Error al recuperar el pedido creado.");
            }

            // 4. Mapeamos a PedidoReadDto
            return new PedidoReadDto
            {
                Id = pedidoGuardado.Id,
                UsuarioId = pedidoGuardado.UsuarioId,
                UsuarioNombre = pedidoGuardado.Usuario != null 
                    ? $"{pedidoGuardado.Usuario.Nombre} ".Trim() 
                    : string.Empty,
                FechaPedido = pedidoGuardado.FechaPedido,
                Total = pedidoGuardado.Total,

                
                DetallesPedidos = pedidoGuardado.DetallesPedidos?.Select(d => new DetallePedidoReadDto
                {
                    Id = d.Id,
                    LibroId = d.LibroId,
                    LibroTitulo = d.Libro != null ? d.Libro.Titulo : string.Empty,
                    PrecioUnitario = d.PrecioUnitario,
                    Cantidad = d.Cantidad
                    
                }).ToList() ?? new List<DetallePedidoReadDto>()
            };
        }

        public async Task<bool> UpdateEstadoAsync(int id, string nuevoEstado)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null) return false;

            pedido.Estado = nuevoEstado;

            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}