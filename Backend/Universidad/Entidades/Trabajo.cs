using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Trabajo
    {
        [Key]
        public int Id { get; set; }
        public string CodigoTrabajo { get; set; }
        public string CodigoRequisito { get; set; }
        public decimal SalarioBase { get; set; } 
        public string TipoTrabajo { get; set; }
        public bool Estado { get; set; }
    }
}