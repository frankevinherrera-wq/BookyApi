
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using BookfyApi.Data;
using BookfyApi.DTOs.Usuario;
using BookfyApi.Services.Implementations;
using Xunit;

namespace MiBook.Tests.Services;

public class AuthServiceTests
{
    private ApplicationDbContext GetDbContextInMemory()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private IConfiguration GetTestConfiguration()
    {
        var values = new Dictionary<string, string?>
        {
            ["Jwt:SecretKey"] = "esta-es-una-clave-de-prueba-de-32-caracteres",
            ["Jwt:Issuer"] = "BookfyTest",
            ["Jwt:Audience"] = "BookfyTest"
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();
    }

    [Fact]
    public async Task RegisterAsync_CuandoUsuarioEsNuevo_DebeCrearUsuario()
    {
        // Arrange
        using var context = GetDbContextInMemory();
        var config = GetTestConfiguration();
        var logger = NullLogger<AuthService>.Instance;

        var servicio = new AuthService(
            context,
            config,
            logger
        );

        var dto = new UsuarioCreateDto
        {
            Nombre = "Juan Prueba",
            Email = "juan@test.com",
            Contrasena = "Clave123!"
        };

        // Act
        var resultado = await servicio.RegisterAsync(dto);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("Juan Prueba", resultado.Nombre);
        Assert.Equal("juan@test.com", resultado.Email);

        var usuarioGuardado = await context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == "juan@test.com");

        Assert.NotNull(usuarioGuardado);
        Assert.NotEqual("Clave123!", usuarioGuardado.Contrasena);
    }

    [Fact]
    public async Task LoginAsync_CuandoCredencialesSonCorrectas_DebeRetornarToken()
    {
        // Arrange
        using var context = GetDbContextInMemory();
        var config = GetTestConfiguration();
        var logger = NullLogger<AuthService>.Instance;

        var servicio = new AuthService(
            context,
            config,
            logger
        );

        await servicio.RegisterAsync(new UsuarioCreateDto
        {
            Nombre = "Juan Prueba",
            Email = "juan@test.com",
            Contrasena = "Clave123!"
        });

        var usuarioGuardado = await context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == "juan@test.com");

        Assert.NotNull(usuarioGuardado);
        Assert.NotNull(usuarioGuardado.Contrasena);
        Assert.NotEqual("Clave123!", usuarioGuardado.Contrasena);

        Assert.True(
            BCrypt.Net.BCrypt.Verify(
                "Clave123!",
                usuarioGuardado.Contrasena
            )
        );

        var dto = new UsuarioLoginDto
        {
            Email = "juan@test.com",
            Contrasena = "Clave123!"
        };

        // Act
        var token = await servicio.LoginAsync(dto);

        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }

    [Fact]
    public async Task LoginAsync_CuandoContrasenaEsIncorrecta_DebeLanzarExcepcion()
    {
        // Arrange
        using var context = GetDbContextInMemory();
        var config = GetTestConfiguration();
        var logger = NullLogger<AuthService>.Instance;

        var servicio = new AuthService(
            context,
            config,
            logger
        );

        await servicio.RegisterAsync(new UsuarioCreateDto
        {
            Nombre = "Juan Prueba",
            Email = "juan@test.com",
            Contrasena = "Clave123!"
        });

        var dto = new UsuarioLoginDto
        {
            Email = "juan@test.com",
            Contrasena = "ClaveIncorrecta!"
        };

        // Act + Assert
        var excepcion = await Assert.ThrowsAsync<InvalidOperationException>(
            () => servicio.LoginAsync(dto)
        );

        Assert.Equal("Credenciales inválidas.", excepcion.Message);
    }
}
