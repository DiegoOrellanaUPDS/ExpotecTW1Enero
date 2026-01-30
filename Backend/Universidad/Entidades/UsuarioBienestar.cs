using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class UsuarioBienestar
    {
        [Key]
        public int Id { get; set; }
        
        public string CodigoUsuario { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        [MaxLength(100)]
        public string NombreUsuario { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string NombreCompleto { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Correo { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Rol { get; set; } = "usuario";
        
        public DateOnly FechaCreacion { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        
        [MaxLength(20)]
        public string Estado { get; set; } = "activo";
        
        [MaxLength(50)]
        public string? Especialidad { get; set; }
        
        [MaxLength(20)]
        public string? Telefono { get; set; }
        
        public string? FotoUrl { get; set; }
    }
}