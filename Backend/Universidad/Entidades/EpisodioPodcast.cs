using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class EpisodioPodcast
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string CodigoEpisodio { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Categoria { get; set; } // "educativo", "entrevista", "debate", "noticias"
        
        [Required]
        [MaxLength(1000)]
        public string Descripcion { get; set; }
        
        [Required]
        public int UsuarioPodcastId { get; set; } // Productor principal
        
        public DateOnly FechaGrabacion { get; set; }
        
        public DateOnly? FechaPublicacion { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } = "planificado"; // "planificado", "grabado", "editado", "publicado", "archivado"
        
        [MaxLength(200)]
        public string? Invitados { get; set; }
        
        public int DuracionMinutos { get; set; }
        
        public string? AudioUrl { get; set; }
        
        public string? ImagenUrl { get; set; }
        
        [MaxLength(500)]
        public string? NotasEpisodio { get; set; }
        
        public bool DisponibleDescarga { get; set; } = true;
        
        public int Reproducciones { get; set; } = 0;
        
        public DateOnly FechaCreacion { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}