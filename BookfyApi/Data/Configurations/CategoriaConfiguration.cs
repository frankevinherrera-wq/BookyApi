
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookfyApi.Models;
using System.Security.Cryptography.X509Certificates;

namespace BookfyApi.Data.Configurations;

public class CategoriaConfigurations : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nombre)
        .IsRequired()
        .HasMaxLength(100);
    }
}