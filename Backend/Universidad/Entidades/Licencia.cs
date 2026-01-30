using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Licencia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int estudianteId { get; set; } // De Estudiante.cs

        [Required]
        public int docenteId { get; set; } // De Docente.cs

        [Required]
        public int carreraId { get; set; } // De Carrera.cs

        [Required]
        public int materiaId { get; set; } // De Materia.cs

        public string motivo { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public string estado { get; set; } = "Pendiente"; 
        public string observaciones { get; set; }
    }
}