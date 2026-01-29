    using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class Materia
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public bool Estado { get; set; }
    }
}