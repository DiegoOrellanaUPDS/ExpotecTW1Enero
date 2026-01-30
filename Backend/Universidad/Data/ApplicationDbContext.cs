using Microsoft.EntityFrameworkCore;
using Universidad.Models;

namespace Universidad.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Estudiante> Estudiantes { get; set; }
    public DbSet<Materia> Materias { get; set; }
    public DbSet<Inscripcion> Inscripciones { get; set; }
    public DbSet<Grupo> Grupos { get; set; }
    public DbSet<Docente> Docentes { get; set; }
    public DbSet<Proyeccion> Proyecciones { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Estudiante configuration
        modelBuilder.Entity<Estudiante>()
            .HasIndex(e => e.Codigo)
            .IsUnique();

        modelBuilder.Entity<Estudiante>()
            .HasIndex(e => e.CI)
            .IsUnique();

        // Materia configuration
        modelBuilder.Entity<Materia>()
            .HasIndex(m => m.Codigo)
            .IsUnique();

        // Grupo configuration
        modelBuilder.Entity<Grupo>()
            .HasIndex(g => new { g.MateriaId, g.Paralelo, g.Turno })
            .IsUnique();

        // Relationships
        modelBuilder.Entity<Inscripcion>()
            .HasOne(i => i.Estudiante)
            .WithMany(e => e.Inscripciones)
            .HasForeignKey(i => i.EstudianteId);

        modelBuilder.Entity<Inscripcion>()
            .HasOne(i => i.Materia)
            .WithMany()
            .HasForeignKey(i => i.MateriaId);

        modelBuilder.Entity<Inscripcion>()
            .HasOne(i => i.Grupo)
            .WithMany(g => g.Inscripciones)
            .HasForeignKey(i => i.GrupoId);

        modelBuilder.Entity<Grupo>()
            .HasOne(g => g.Materia)
            .WithMany(m => m.Grupos)
            .HasForeignKey(g => g.MateriaId);

        modelBuilder.Entity<Grupo>()
            .HasOne(g => g.Docente)
            .WithMany(d => d.GruposAsignados)
            .HasForeignKey(g => g.DocenteId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Proyeccion>()
            .HasOne(p => p.Estudiante)
            .WithMany()
            .HasForeignKey(p => p.EstudianteId);
    }
}
