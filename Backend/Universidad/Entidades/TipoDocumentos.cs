using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class TipoDocumentos
    {
        [Key]
        public int documentoId {get;set;}
        public string nombreDocumento {get;set;}
        public string codigoDocumento {get;set;}    
        public string aplicacion {get;set;}
        public string obligatorio {get;set;}
        public string descripcion {get;set;}
        public string estado {get;set;} = "activo";

    }
}