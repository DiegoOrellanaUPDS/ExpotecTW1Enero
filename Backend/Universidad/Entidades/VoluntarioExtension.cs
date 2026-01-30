using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class VoluntarioExtension
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string NombreCompleto { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Correo { get; set; }
        
        [MaxLength(20)]
        public string Telefono { get; set; }
        
        [MaxLength(50)]
        public string AreaInteres { get; set; }
        
        public int HorasVoluntariado { get; set; } = 0;
        
        [MaxLength(20)]
        public string Estado { get; set; } = "activo";
        
        public DateOnly FechaRegistro { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}