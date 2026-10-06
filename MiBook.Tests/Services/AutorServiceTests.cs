using Xunit;
using Microsoft.EntityFrameworkCore;
using BookfyApi.Data;
using BookfyApi.Models;
using BookfyApi.DTOs.Autor;
using BookfyApi.Services.Implementations;

namespace MiBook.Tests.Services;

public class AutorServiceTests
{
    private ApplicationDbContext GetDbContextInMemory()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task GetByIdAsync_CuandoElAutorExiste_DebeRetornarElAutorReadDto()
    {
        using var context = GetDbContextInMemory();
        
        var autorFalso = new Autor { Id = 1, Nombre = "Gabriel Garcia Marquez", Nacionalidad = "Colombian" };
        context.Autores.Add(autorFalso);
        await context.SaveChangesAsync();

        var servicio = new AutorService(context);

        var resultado = await servicio.GetByIdAsync(1);

        Assert.NotNull(resultado);
        Assert.Equal(1, resultado.Id);
        Assert.Equal("Gabriel Garcia Marquez", resultado.Nombre);
        Assert.Equal("Colombian", resultado.Nacionalidad);
    }

    [Fact]
    public async Task GetByIdAsync_CuandoElAutorNoExiste_DebeRetornarNull()
    {
        using var context = GetDbContextInMemory();
        var servicio = new AutorService(context);

        var resultado = await servicio.GetByIdAsync(4); 

        Assert.Null(resultado);
    }


    
}