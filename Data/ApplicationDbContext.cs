using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookfyApi.Models;
using Microsoft.EntityFrameworkCore;


namespace BookfyApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options ) : base(options)
        {
        }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Autor> Autores { get; set; }
        
    }
}