using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Estudiante
    {
        [Key]
        public int estudianteId {get;set;}
        public string nombreEstudiante {get;set;}
        public string apellidoEstudiante {get;set;}
        public string estudianteCi {get;set;}
        public DateOnly FechaDeNacimiento {get;set;}
        public string genero {get;set;}
        public string emailPersonal {get;set;}
        public string emailInstitucional {get;set;}
        public string telefono {get;set;}
        public string direccion {get;set;}
        public string carrera {get;set;}
        public DateOnly fechaDeIngreso {get;set;}
        public int pocentajeDeBeca {get;set;}
        public string tipoDeBeca {get;set;}
        public string estado {get;set;}="activo";
    }
}