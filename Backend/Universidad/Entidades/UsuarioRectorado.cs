using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entidades
{
    public class UsuarioRectorado
    {
        [Key]
        public int Id { get; set; }
        
        public string Nombre { get; set; } = string.Empty;
        
        public string Correo { get; set; } = string.Empty;
        
        public string Rol { get; set; } = "Admin Rectorado"; 
        
        public string TokenSesion { get; set; } = string.Empty; // Token para validar sesión
        
        public string Estado { get; set; } = "Activo";
    }
}