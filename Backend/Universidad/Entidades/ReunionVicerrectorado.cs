using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class ReunionVicerrectorado
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string CodigoReunion { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string TipoReunion { get; set; } // "ordinaria", "extraordinaria", "emergencia", "comision"
        
        [Required]
        [MaxLength(1000)]
        public string Agenda { get; set; }
        
        public DateOnly FechaReunion { get; set; }
        
        [MaxLength(50)]
        public string HoraInicio { get; set; }
        
        [MaxLength(50)]
        public string HoraFin { get; set; }
        
        [MaxLength(200)]
        public string Lugar { get; set; }
        
        [Required]
        public int UsuarioVicerrectoradoId { get; set; }
        
        [MaxLength(20)]
        public string Estado { get; set; } = "programada"; // "programada", "en_curso", "realizada", "cancelada"
        
        [MaxLength(500)]
        public string? Participantes { get; set; }
        
        [MaxLength(500)]
        public string? Acuerdos { get; set; }
        
        [MaxLength(500)]
        public string? MinutaUrl { get; set; }
        
        public DateOnly FechaCreacion { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}