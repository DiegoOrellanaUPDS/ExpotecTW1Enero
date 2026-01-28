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
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<ActividadDepa> ActividadesDepa { get; set; }
    }
}