using Microsoft.EntityFrameworkCore;
using Entidades;
using Entities;

namespace Universidad.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Auditoria> Auditorias {get;set;}
        public DbSet<Carrera> Carreras {get;set;}
        public DbSet<Docente> Docentes {get;set;}
        public DbSet<Estudiante> Estudiantes {get;set;}
        public DbSet<ExpedienteDigital> ExpedientesDigitales {get;set;}
        public DbSet<Inscripcion> Inscripciones {get;set;}
        public DbSet<TipoDocumentos> TiposDocumentos {get;set;}
        public DbSet<UsuarioConsistencia> UsuariosConsistencia {get;set;}
        
    }
}