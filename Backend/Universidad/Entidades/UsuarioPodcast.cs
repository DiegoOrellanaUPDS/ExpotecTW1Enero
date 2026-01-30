using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class UsuarioPodcast
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
        public string Rol { get; set; } = "productor"; // "productor", "editor", "presentador", "invitado"
        
        public DateOnly FechaCreacion { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        
        [MaxLength(20)]
        public string Estado { get; set; } = "activo";
        
        [MaxLength(50)]
        public string? Especialidad { get; set; } // "tecnico", "contenido", "marketing"
        
        [MaxLength(20)]
        public string? Telefono { get; set; }
        
        public string? FotoUrl { get; set; }
        
        [MaxLength(200)]
        public string? Biografia { get; set; }
    }
}