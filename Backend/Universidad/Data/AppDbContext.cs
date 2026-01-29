using Microsoft.EntityFrameworkCore;
using Entidades;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
           
        }

        public DbSet<Usuario_Caja> Usuarios_Caja { get; set; }
        public DbSet<UsuarioFI> UsuarioFIs { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<ContabilidadPeticionDepartamento> ContabilidadPeticionDepartamentos { get; set; }
        public DbSet<ContabilidadReportesIngresos> ContabilidadReportesIngresos { get; set; }
        public DbSet<Libros> Libros { get; set; }
        public DbSet<Categorias> Categorias { get; set; }
        public DbSet<Prestamos> Prestamos { get; set; }
        public DbSet<Universidad.Entidades.LimpiezaInsumo> LimpiezaInsumos { get; set; }
        public DbSet<Universidad.Entidades.ObjetoPerdido> ObjetosPerdidos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<ActividadDepa> ActividadesDepa { get; set; }
        public DbSet<Solicitud> Solicitudes { get; set; }
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
                    .HasDefaultValueSql("CURRENT_DATE");
            });
            base.OnModelCreating(modelBuilder);

            // DataTime (C#) == Date (PostreSQL)

            // Recorre todas las entidades y propiedades DateTime
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    // Si la propiedad es DateTime o DateTime?
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetColumnType("date"); // Se guarda como "date" en PostgreSQL
                    }
                }
            }
        }

        public DbSet<Auditoria> Auditorias {get;set;}
        public DbSet<Carrera> Carreras {get;set;}

        public DbSet<Docente> Docentes {get;set;}

        public DbSet<Estudiante> Estudiantes {get;set;}
        public DbSet<ExpedienteDigital> ExpedientesDigitales {get;set;}
        public DbSet<Inscripcion> Inscripciones {get;set;}
        public DbSet<TipoDocumentos> TiposDocumentos {get;set;}
        public DbSet<UsuarioConsistencia> UsuariosConsistencia {get;set;}
        
        //ProduccionAudiovisual
        public DbSet<Universidad.Entidades.PersonaProduccion> PersonaProduccions { get; set; }
        public DbSet<Universidad.Entidades.SolicitudProduccion> SolicitudProduccions { get; set; }
        public DbSet<Universidad.Entidades.ProduccionAudiovisual> ProduccionAudiovisuals { get; set; }
        public DbSet<Universidad.Entidades.HistorialCambios> HistorialCambioses { get; set; }
        public DbSet<Universidad.Entidades.Cancelacion> Cancelaciones { get; set; }



    }

}

