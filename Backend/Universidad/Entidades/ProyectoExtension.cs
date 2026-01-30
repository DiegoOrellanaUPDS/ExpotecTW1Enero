using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class ProyectoExtension
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string CodigoProyecto { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; }
        
        [Required]
        [MaxLength(1000)]
        public string Descripcion { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Responsable { get; set; }
        
        public DateOnly FechaInicio { get; set; }
        
        public DateOnly? FechaFin { get; set; }
        
        [MaxLength(20)]
        public string Estado { get; set; } = "planificado";
        
        public decimal Presupuesto { get; set; }
        
        [MaxLength(500)]
        public string Beneficiarios { get; set; }
        
        public DateOnly FechaCreacion { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}