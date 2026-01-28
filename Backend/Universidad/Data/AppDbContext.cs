using Microsoft.EntityFrameworkCore;
using Entidades;

namespace Universidad.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Define tus DbSet aquí
        public DbSet<Persona> Estudiantes { get; set; }
        public DbSet<Libros> Libros { get; set; }
        public DbSet<Categorias> Categorias { get; set; }
        public DbSet<Prestamos> Prestamos { get; set; }
    }
}