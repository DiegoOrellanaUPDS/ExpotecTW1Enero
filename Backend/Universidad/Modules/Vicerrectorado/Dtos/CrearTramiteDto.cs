using System.ComponentModel.DataAnnotations;
using Universidad.Modules.Vicerrectorado.Models;

namespace Universidad.Modules.Vicerrectorado.Dtos;

public class CrearTramiteDto
{
    [Required]
    public TipoTramite Tipo { get; set; }

    [Required, MaxLength(2000)]
    public string Descripcion { get; set; } = string.Empty;
}
