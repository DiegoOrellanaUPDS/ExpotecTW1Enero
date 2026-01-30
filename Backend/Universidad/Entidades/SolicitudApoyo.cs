using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class SolicitudApoyo
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string CodigoSolicitud { get; set; }
        
        [Required]
        public int EstudianteBeneficiarioId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string TipoSolicitud { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string Descripcion { get; set; }
        
        public DateOnly FechaSolicitud { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        
        public DateOnly? FechaAtencion { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Prioridad { get; set; } = "media";
        
        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } = "pendiente";
        
        public int? UsuarioBienestarId { get; set; }
        
        [MaxLength(500)]
        public string? Observaciones { get; set; }
        
        [MaxLength(50)]
        public string? CanalAtencion { get; set; }
        
        public bool SeguimientoRequiere { get; set; } = false;
        
        public DateOnly? ProximaSesion { get; set; }
    }
}