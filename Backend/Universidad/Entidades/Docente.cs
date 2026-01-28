using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Docente
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public int Telefono { get; set; }
        public string Codigo { get; set; }
        public DateOnly FechaContratacion { get; set; }
        public bool Estado { get; set; } = true;
    }
}