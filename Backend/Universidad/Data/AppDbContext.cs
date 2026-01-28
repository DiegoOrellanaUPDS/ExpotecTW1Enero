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
        public DbSet<Paciente> Pacientes { get; set; }
    }
}