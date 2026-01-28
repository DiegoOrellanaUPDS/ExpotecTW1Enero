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


        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<Evaluacion> Evaluaciones {get;set;}
        public DbSet<Postulante> Postulantes {get;set;}
        public DbSet<Reclutador> Reclutadores {get;set;}
        public DbSet<RequisitoMinimo> RequisitoMinimos {get;set;}
        public DbSet<Trabajo> Trabajos {get;set;}
        public DbSet<UsuarioTH> UsuarioTHs {get;set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Profesor>(entity =>
            {
                entity.ToTable("Profesores");
                entity.HasKey(e => e.Id);
            
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(e => e.Categoria)
                    .IsRequired()
                    .HasMaxLength(50);
                
                entity.Property(e => e.Correo)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.Property(e => e.Especialidad)
                    .HasMaxLength(200);
                
                entity.Property(e => e.FechaCreacion)
                    .HasDefaultValueSql("GETDATE()");
            });

            base.OnModelCreating(modelBuilder);
        }
    }

}

