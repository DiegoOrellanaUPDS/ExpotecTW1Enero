
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Carrera
    {
        [Key]
        public int carreraId {get;set;}
        public string nombreCarrera {get;set;}
        public string codigoCarrera {get;set;}
        public string facultad {get;set;}
        public string estado {get;set;} = "activo";
    }
}

