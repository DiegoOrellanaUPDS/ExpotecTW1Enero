using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Inscripcion
    {
        [Key]
        public int IdInscripcion {get;set;}
        public int estudianteCi {get;set;}
        public string codigoCarrera {get;set;}
        public string codigoDeInscripcion {get;set;}
        public DateOnly fechaDeInscripcion {get;set;}
        public string estado {get;set;}="activo";

    }
}