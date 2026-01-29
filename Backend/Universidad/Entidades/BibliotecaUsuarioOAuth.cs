using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Universidad.Entidades
{
    [Table("biblioteca_usuarios_oauth")]
    public class BibliotecaUsuarioOAuth
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        public string Rol { get; set; } = "biblioteca";

        public string? Token { get; set; }

        public string? DiscordId { get; set; }
    }
}
