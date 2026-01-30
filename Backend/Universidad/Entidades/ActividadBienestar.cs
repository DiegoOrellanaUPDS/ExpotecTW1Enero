using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class ActividadBienestar
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string CodigoActividad { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string TipoActividad { get; set; }
        
        [Required]
        [MaxLength(1000)]
        public string Descripcion { get; set; }
        
        public DateOnly FechaActividad { get; set; }
        
        [MaxLength(50)]
        public string HoraInicio { get; set; }
        
        [MaxLength(50)]
        public string HoraFin { get; set; }
        
        [MaxLength(200)]
        public string Lugar { get; set; }
        
        [Required]
        public int UsuarioBienestarId { get; set; }
        
        [MaxLength(20)]
        public string Estado { get; set; } = "planificada";
        
        public int CapacidadMaxima { get; set; }
        
        public int InscritosActuales { get; set; } = 0;
        
        [MaxLength(500)]
        public string? MaterialesRequeridos { get; set; }
        
        public bool CertificadoDisponible { get; set; } = false;
        
        [MaxLength(500)]
        public string? Observaciones { get; set; }
        
        public DateOnly FechaCreacion { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}