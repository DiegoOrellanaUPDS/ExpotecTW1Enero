using System.ComponentModel.DataAnnotations;

namespace Universidad.Modules.Vicerrectorado.Models;

public enum EstadoConvocatoria
{
    Borrador = 0,
    Publicada = 1,
    Cerrada = 2
}

public class Convocatoria
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(160)]
    public string Titulo { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Descripcion { get; set; }

    public DateTime FechaPublicacion { get; set; } = DateTime.UtcNow;
    public DateTime? FechaCierre { get; set; }

    public EstadoConvocatoria Estado { get; set; } = EstadoConvocatoria.Borrador;

    [MaxLength(80)]
    public string CreadoPorUserId { get; set; } = string.Empty;
}