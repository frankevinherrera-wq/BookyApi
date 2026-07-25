using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using BookfyApi.Models;


namespace BookfyApi.Data.Configurations;
public class LibroConfiguration : IEntityTypeConfiguration<Libro>
{
    public void Configure(EntityTypeBuilder<Libro> builder)
    {
        builder.HasKey(l => l.Id);

        
        builder.Property(l => l.Titulo)
        .IsRequired()
        .HasMaxLength(100);

        builder.Property(l => l.AnioPublicacion)
        .IsRequired();
    
       

        builder.HasOne(l => l.Autor)
            .WithMany(a => a.Libros)
            .HasForeignKey(l => l.AutorId)
            .OnDelete(DeleteBehavior.Restrict); 

        builder.HasOne(l => l.Categoria)
            .WithMany(c => c.Libros)
            .HasForeignKey(l => l.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}