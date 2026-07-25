using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using BookfyApi.Models;

namespace BookfyApi.Data.Configurations;

public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasOne(p => p.Usuario)
            .WithMany(u => u.Pedidos)
            .HasForeignKey(p => p.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(p => p.FechaPedido)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");
        
        builder.Property(p => p.Total)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0);
    }
}
