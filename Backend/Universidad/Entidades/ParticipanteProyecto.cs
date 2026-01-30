using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class ParticipanteProyecto
    {
        [Key]
        public int IdRegistroParticipanteProyecto { get; set; }
        public string CodigoParticipanteProyecto { get; set; } = string.Empty;
        public int ProyectoIdParticipanteProyecto { get; set; }
        public string TipoParticipanteProyecto { get; set; } = string.Empty;
        public string NombreCompletoParticipanteProyecto { get; set; } = string.Empty;
        public string RolParticipanteProyecto { get; set; } = string.Empty;
        public string? CorreoParticipanteProyecto { get; set; }
        public string? TelefonoParticipanteProyecto { get; set; }
        public string EstadoParticipanteProyecto { get; set; } = "activo";
        public DateOnly FechaInscripcionParticipanteProyecto { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}