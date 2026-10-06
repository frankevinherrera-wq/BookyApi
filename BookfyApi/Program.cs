using Microsoft.EntityFrameworkCore;
using BookfyApi.Data;
using BookfyApi.Services.Interfaces;
using BookfyApi.Services.Implementations;


using BookfyApi.Middlewares;
using BookfyApi.Models;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=bookfy.db") );

builder.Services.AddControllers();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IAutorService , AutorService>();

builder.Services.AddScoped<ILibroService, LibroService>();

builder.Services.AddScoped<IPedidoService , PedidoService>();

builder.Services.AddScoped<IAuthService, AuthService>(); 
builder.Services.AddScoped<IUsuarioService , UsuarioService>();


var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>(); 
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}



// -> ORDEN CRÍTICO DE AUTHENTICATION Y AUTHORIZATION
// Debe ir siempre antes de MapControllers() y en este orden específico:
app.UseAuthentication(); // 1º Lee el header Bearer, descifra los claims y construye HttpContext.User
app.UseAuthorization();  // 2º Evalúa si el usuario tiene permiso ([Authorize], [Authorize(Roles = "Admin")])

app.MapControllers();

app.Run();

