// Entidades/ContabilidadLoginOauth.cs
using System.ComponentModel.DataAnnotations;

namespace Universidad.Entidades
{
    public class ContabilidadLoginOauth
    {
        [Key]
        public int IdRegistroContabilidadLoginOauth { get; set; }
        public string IdentificadorDiscordUsuarioContabilidad { get; set; } = string.Empty;
        public string TokenSistemaUsuarioContabilidad { get; set; } = string.Empty;
        public string NombreCompletoUsuarioContabilidad { get; set; } = string.Empty;
        public DateTime FechaCreacionUsuarioContabilidad { get; set; } = DateTime.UtcNow;
    }
}