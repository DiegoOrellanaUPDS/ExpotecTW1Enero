using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class UsuarioTH
    {
        [Key]
        public int Id{get;set;}
        public string NombreUsuario {get;set;}
        public string Token{get;set;}
        public string Rol {get;set;}
        public string Estado {get;set;}
    }
}