using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class ProgramacionPodcast
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string CodigoProgramacion { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string TipoPrograma { get; set; } // "seriado", "especial", "temporada"
        
        [Required]
        [MaxLength(500)]
        public string Descripcion { get; set; }
        
        public DateOnly FechaInicio { get; set; }
        
        public DateOnly? FechaFin { get; set; }
        
        [Required]
        public int UsuarioPodcastId { get; set; } // Coordinador
        
        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } = "activo"; // "activo", "pausado", "finalizado"
        
        public int TotalEpisodios { get; set; } = 0;
        
        public int EpisodiosPublicados { get; set; } = 0;
        
        [MaxLength(200)]
        public string? Temas { get; set; }
        
        [MaxLength(500)]
        public string? PublicoObjetivo { get; set; }
        
        public string? ImagenProgramaUrl { get; set; }
        
        [MaxLength(500)]
        public string? Observaciones { get; set; }
        
        public DateOnly FechaCreacion { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}