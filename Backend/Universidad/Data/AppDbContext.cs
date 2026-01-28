using Microsoft.EntityFrameworkCore;
using Entidades;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Define tus DbSet aquí
        public DbSet<Persona> Estudiantes { get; set; }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<ActividadDepa> ActividadesDepa { get; set; }
        public DbSet<Solicitud> Solicitudes { get; set; }
        

    }

}

