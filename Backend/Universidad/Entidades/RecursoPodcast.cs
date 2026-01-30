using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class RecursoPodcast
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string CodigoRecurso { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Nombre { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string TipoRecurso { get; set; } // "equipo", "software", "estudio", "accesorio"
        
        [Required]
        [MaxLength(500)]
        public string Descripcion { get; set; }
        
        [Required]
        public int UsuarioPodcastId { get; set; } // Responsable
        
        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } = "disponible"; // "disponible", "en_uso", "mantenimiento", "dañado"
        
        [MaxLength(100)]
        public string? Marca { get; set; }
        
        [MaxLength(100)]
        public string? Modelo { get; set; }
        
        [MaxLength(50)]
        public string? Serial { get; set; }
        
        public DateOnly? FechaAdquisicion { get; set; }
        
        public decimal? Valor { get; set; }
        
        [MaxLength(500)]
        public string? Caracteristicas { get; set; }
        
        public string? ImagenRecursoUrl { get; set; }
        
        [MaxLength(500)]
        public string? Observaciones { get; set; }
        
        public DateOnly FechaRegistro { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}