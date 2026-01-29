using System.ComponentModel.DataAnnotations;

namespace Entidades

{
    public class Docente
    {
        [Key]

        public int docenteId {get;set;}
        public string nombreDocente {get;set;}
        public string apellidoDocente {get;set;}
        public string docenteCi {get;set;}
        public DateOnly fechaDeNacimiento {get;set;}
        public string genero {get;set;}
        public string emailPersonal {get;set;}
        public string emailInstitucional {get;set;}
        public string telefono {get;set;}
        public string direccion {get;set;}
        public string gradoAcademido {get;set;}
        public string fechaDeIngreso {get;set;}
        public bool estado {get;set;}=true;

    }
}