using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entidades
{
    public class UsuarioRectorado
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Rol { get; set; } = "Admin"; 
        public string Token { get; set; } = string.Empty; // Token de sesión propio
        public string Estado { get; set; } = "Activo";
    }
}