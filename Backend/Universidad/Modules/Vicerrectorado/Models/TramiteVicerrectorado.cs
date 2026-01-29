using System.ComponentModel.DataAnnotations;

namespace Universidad.Modules.Vicerrectorado.Models;

public enum TipoTramite
{
    SolicitudCertificacion = 0,
    CartaPresentacion = 1,
    AvalInstitucional = 2,
    Otro = 99
}

public enum EstadoTramite
{
    Registrado = 0,
    EnRevision = 1,
    Aprobado = 2,
    Rechazado = 3
}

public class TramiteVicerrectorado
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public TipoTramite Tipo { get; set; } = TipoTramite.Otro;

    [Required, MaxLength(2000)]
    public string Descripcion { get; set; } = string.Empty;

    public DateTime FechaSolicitud { get; set; } = DateTime.UtcNow;

    public EstadoTramite Estado { get; set; } = EstadoTramite.Registrado;

    [MaxLength(80)]
    public string SolicitanteUserId { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Observacion { get; set; }
}
