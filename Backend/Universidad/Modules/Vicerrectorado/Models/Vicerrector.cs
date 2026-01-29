using System.ComponentModel.DataAnnotations;

namespace Universidad.Modules.Vicerrectorado.Models;

public class Vicerrector
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(120)]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(120)]
    public string Email { get; set; } = string.Empty;

    public DateTime FechaInicio { get; set; } = DateTime.UtcNow.Date;

    public bool Activo { get; set; } = true;
}