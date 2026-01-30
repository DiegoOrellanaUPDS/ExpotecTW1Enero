using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class ResolucionVicerrectorado
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string NumeroResolucion { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string TipoResolucion { get; set; } // "academica", "administrativa", "designacion", "normativa"
        
        [Required]
        [MaxLength(2000)]
        public string Contenido { get; set; }
        
        public DateOnly FechaEmision { get; set; }
        
        public DateOnly? FechaVigencia { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } = "vigente"; // "vigente", "derogada", "suspendida"
        
        public int UsuarioVicerrectoradoId { get; set; }
        
        [MaxLength(500)]
        public string? Fundamentos { get; set; }
        
        [MaxLength(500)]
        public string? Alcance { get; set; }
        
        public string? DocumentoUrl { get; set; }
        
        public bool RequiereFirmaDigital { get; set; } = false;
        
        public DateOnly FechaCreacion { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}