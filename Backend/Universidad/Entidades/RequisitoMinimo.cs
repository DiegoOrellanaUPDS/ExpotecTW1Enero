using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class RequisitoMinimo
    {
        [Key]
        public int Id { get; set; }
        public string CodigoRequisito { get; set; }
        public string Requisito { get; set; }
        public bool Estado { get; set; } 
    }
}