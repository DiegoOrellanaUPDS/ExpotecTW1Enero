using Microsoft.EntityFrameworkCore;
using Universidad.Modules.Vicerrectorado.Models;
using Entidades;

namespace Universidad.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // ===== Tu entidad existente =====
        public DbSet<Persona> Estudiantes { get; set; } = null!;

        // ===== MÓDULO VICERRECTORADO =====
        public DbSet<Vicerrector> Vicerrectores { get; set; } = null!;
        public DbSet<Convocatoria> VicerrectoradoConvocatorias { get; set; } = null!;
        public DbSet<TramiteVicerrectorado> VicerrectoradoTramites { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== MÓDULO VICERRECTORADO =====
            modelBuilder.Entity<Vicerrector>().ToTable("vicerrectores");
            modelBuilder.Entity<Convocatoria>().ToTable("vicerrectorado_convocatorias");
            modelBuilder.Entity<TramiteVicerrectorado>().ToTable("vicerrectorado_tramites");

            modelBuilder.Entity<Vicerrector>()
                .HasIndex(x => x.Email)
                .IsUnique();
        }
    }
}