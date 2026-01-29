using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Carta
    {
        [Key]
        public int Id { get; set; }
        public string Codigo { get; set; } // Valor para referencias
        public string EstudianteCI { get; set; }
        public string NITEmpresa { get; set; }
        public DateOnly FechaGeneracion { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string? FolioCarta { get; set; } // otro identificador de la carta
        public string? Url { get; set; } // por si se almacenan en la nube
        public string Estado { get; set; }
    }
}