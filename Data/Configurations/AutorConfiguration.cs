using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BookfyApi.Models;


namespace BookfyApi.Data.Configurations;
public class AutorConfigurations : IEntityTypeConfiguration <Autor>{
    
    public void Configure(EntityTypeBuilder<Autor> builder)
    {
        builder.HasKey(a => a.Id);


        builder.Property(a => a.Nombre)
        .IsRequired()
        .HasMaxLength(100);

        builder.Property(a => a.Nacionalidad)
        .IsRequired()
        .HasMaxLength(100);
    }
}