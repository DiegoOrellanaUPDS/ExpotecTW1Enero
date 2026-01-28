using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Auditoria
    {
        [Key]
        public int idAuditoria {get;set;}
        public string codigoDocumento {get;set;}
        public string  codigoUsuario {get;set;}
        public string obeservaciones {get;set;}
        public DateOnly fechaDeModificacion {get;set;}


    }
}