using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class EstudianteBeneficiario
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string CI { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string NombreCompleto { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Carrera { get; set; }
        
        public int Semestre { get; set; }
        
        [MaxLength(100)]
        public string Correo { get; set; }
        
        [MaxLength(20)]
        public string Telefono { get; set; }
        
        public DateOnly FechaRegistro { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        
        [MaxLength(20)]
        public string Estado { get; set; } = "activo";
        
        public bool TieneBeca { get; set; } = false;
        
        [MaxLength(50)]
        public string? TipoApoyo { get; set; }
        
        public string? Observaciones { get; set; }
        
        public int? UsuarioBienestarId { get; set; }
    }
}