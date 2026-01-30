using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class InformeVicerrectorado
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string CodigoInforme { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string TipoInforme { get; set; } // "academico", "administrativo", "financiero", "estadistico"
        
        [Required]
        [MaxLength(1000)]
        public string Descripcion { get; set; }
        
        public DateOnly FechaGeneracion { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        
        public DateOnly? FechaAprobacion { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } = "borrador"; // "borrador", "revision", "aprobado", "publicado"
        
        public int UsuarioVicerrectoradoId { get; set; }
        
        [MaxLength(50)]
        public string? PeriodoAcademico { get; set; }
        
        [MaxLength(500)]
        public string? Observaciones { get; set; }
        
        public string? DocumentoUrl { get; set; }
        
        public bool Confidencial { get; set; } = false;
    }
}