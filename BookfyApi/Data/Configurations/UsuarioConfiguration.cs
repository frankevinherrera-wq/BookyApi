using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using BookfyApi.Models;

namespace BookfyApi.Data.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {

        builder.HasKey(p => p.Id);
        
        builder.Property(u => u.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100);
        builder.HasIndex(u => u.Email)
            .IsUnique();
        
        builder.Property(u => u.Contrasena)
            .IsRequired()
            .HasMaxLength(250);
        
        builder.Property(u => u.FechaRegistro)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()");
        
        
    }
}
