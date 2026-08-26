using Microsoft.EntityFrameworkCore;
using BookfyApi.Data;
using BookfyApi.Services.Interfaces;
using BookfyApi.Services.Implementations;


using BookfyApi.Middlewares;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=bookfy.db") );

builder.Services.AddControllers();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IAutorService , AutorService>();

builder.Services.AddScoped<ILibroService, LibroService>();




var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>(); 
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}



app.MapControllers();



app.Run();

