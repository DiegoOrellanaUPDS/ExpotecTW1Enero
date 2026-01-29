using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Postulante
    {
        [Key]
        public int Id { get; set; }
        public string CodigoPostulante { get; set; }
        public string CodigoPersona { get; set; }  
        public string CodigoTrabajo { get; set; }  
        public string Telefono { get; set; }
        public int IdDocumento { get; set; } 
        public bool Estado { get; set; }
    }
}