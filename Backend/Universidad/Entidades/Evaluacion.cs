using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Evaluacion
    {
        [Key]
        public int Id { get; set; } 
        public string CodigoReclutador { get; set; }
        public string CodigoPostulante { get; set; }
        public string CodigoTrabajo { get; set; }
        public int NivelEvaluacion { get; set; }
    }
}