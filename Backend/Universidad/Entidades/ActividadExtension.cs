using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class ActividadExtension
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
        [MaxLength(1000)]
        public string Descripcion { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string TipoActividad { get; set; }
        
        public DateOnly FechaActividad { get; set; }
        
        [MaxLength(200)]
        public string Lugar { get; set; }
        
        [MaxLength(500)]
        public string Participantes { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Responsable { get; set; }
        
        [MaxLength(20)]
        public string Estado { get; set; } = "planificada";
        
        public DateOnly FechaCreacion { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}