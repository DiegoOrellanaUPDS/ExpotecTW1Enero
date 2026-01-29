using System;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Inscripcion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EstudianteId { get; set; }

        [Required]
        public int MateriaId { get; set; }

        [Required]
        [MaxLength(30)]
        public string Periodo { get; set; } = "";

        public DateTime FechaInscripcion { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } = "ACTIVA";

        [MaxLength(200)]
        public string? UsuarioId { get; set; }

        [MaxLength(200)]
        public string? UsuarioEmail { get; set; }
    }
}
