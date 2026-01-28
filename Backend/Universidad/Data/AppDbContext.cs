using Microsoft.EntityFrameworkCore;
using Entidades;

namespace Universidad.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
           
        }

        
        public DbSet<Persona> Estudiantes { get; set; }

         public DbSet<Universidad.Entidades.LimpiezaInsumo> LimpiezaInsumos { get; set; }
          public DbSet<Universidad.Entidades.ObjetoPerdido> ObjetosPerdidos { get; set; }
    }
}