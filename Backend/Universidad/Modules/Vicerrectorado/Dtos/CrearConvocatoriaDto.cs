using System.ComponentModel.DataAnnotations;

namespace Universidad.Modules.Vicerrectorado.Dtos;

public class CrearConvocatoriaDto
{
    [Required, MaxLength(160)]
    public string Titulo { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Descripcion { get; set; }
}
