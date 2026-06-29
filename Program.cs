using Microsoft.EntityFrameworkCore;
using BookfyApi.Data;
using BookfyApi.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=bookfy.db") );

builder.Services.AddControllers();
builder.Services.AddScoped<IAutorService, AutorService>();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();



app.Run();

