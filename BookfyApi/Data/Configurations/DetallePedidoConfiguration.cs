using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookfyApi.Models;

namespace BookfyApi.Data.Configurations;

public class DetallePedidoConfiguration : IEntityTypeConfiguration<DetallePedido>
{
    public void Configure(EntityTypeBuilder<DetallePedido>builder)
    {
        builder.HasKey(dp => dp.Id);

        builder.HasOne(dp => dp.Libro)
        .WithMany(l => l.DetallesPedidos)
        .HasForeignKey(dp => dp.LibroId)
        .OnDelete(DeleteBehavior.Cascade);

         builder.HasOne(dp => dp.Pedido)
        .WithMany(p => p.DetallesPedidos)
        .HasForeignKey(dp => dp.PedidoId)
        .OnDelete(DeleteBehavior.Cascade);

        builder.Property(dp => dp.Cantidad)
            .IsRequired();

        builder.Property(dp => dp.PrecioUnitario)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        

    } 

}