using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class UsuarioConsistencia
    {
        [Key]
        public int usuarioId {get;set;}
        public string codigoUsuario {get;set;}
        public string nombreUsuario {get;set;}
        public string contrasena {get;set;}
        public string rol {get;set;}
        public DateOnly fechaDeCreacion {get;set;}
        public string estado {get;set;} = "activo";

    }
}