using System.ComponentModel.DataAnnotations;
using Universidad.Modules.Vicerrectorado.Models;

namespace Universidad.Modules.Vicerrectorado.Dtos;

public class ActualizarEstadoConvocatoriaDto
{
    [Required]
    public EstadoConvocatoria Estado { get; set; }

    public DateTime? FechaCierre { get; set; }
}
