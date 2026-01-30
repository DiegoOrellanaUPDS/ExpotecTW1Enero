using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Usuario_extuniv
    {
        [Key]
        public int IdRegistroUsuarioExtuniv { get; set; }
        public string IdentificadorDiscordUsuarioExtuniv { get; set; } = string.Empty;
        public string TokenSistemaUsuarioExtuniv { get; set; } = string.Empty;
        public string NombreCompletoUsuarioExtuniv { get; set; } = string.Empty;
        public string CorreoUsuarioExtuniv { get; set; } = string.Empty;
        public string RolUsuarioExtuniv { get; set; } = "usuario";
        public string EstadoUsuarioExtuniv { get; set; } = "activo";
        public DateTime FechaCreacionUsuarioExtuniv { get; set; } = DateTime.UtcNow;
        public DateTime? UltimoAccesoUsuarioExtuniv { get; set; }
    }
}