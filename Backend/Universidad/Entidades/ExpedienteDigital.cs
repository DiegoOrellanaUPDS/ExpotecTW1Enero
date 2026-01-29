using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class ExpedienteDigital
    {
        [Key]
        public int archivoId {get;set;}
        public string codigoDocumento {get;set;}
        public string estudianteCi {get;set;}
        public string DocenteCi {get;set;}
        public string nombreArchivo {get;set;}
        public string archivoCode {get;set;}
        public string observaciones {get;set;}
        public string estado {get;set;} = "activo";
    }
}