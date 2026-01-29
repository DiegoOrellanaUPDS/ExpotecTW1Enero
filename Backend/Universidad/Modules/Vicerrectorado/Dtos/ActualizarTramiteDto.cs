using System.ComponentModel.DataAnnotations;
using Universidad.Modules.Vicerrectorado.Models;

namespace Universidad.Modules.Vicerrectorado.Dtos;

public class ActualizarTramiteDto
{
    [Required]
    public EstadoTramite Estado { get; set; }

    [MaxLength(2000)]
    public string? Observacion { get; set; }
}
