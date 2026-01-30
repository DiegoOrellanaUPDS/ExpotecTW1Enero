using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class SocioExtension
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string NombreOrganizacion { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string TipoOrganizacion { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Contacto { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Correo { get; set; }
        
        [MaxLength(20)]
        public string Telefono { get; set; }
        
        [MaxLength(200)]
        public string Direccion { get; set; }
        
        [MaxLength(100)]
        public string AreaColaboracion { get; set; }
        
        [MaxLength(20)]
        public string Estado { get; set; } = "activo";
        
        public DateOnly FechaRegistro { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}