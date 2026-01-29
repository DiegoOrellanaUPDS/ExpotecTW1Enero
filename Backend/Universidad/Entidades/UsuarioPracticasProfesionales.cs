using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class UsuarioPracticasProfesionales
    {
        [Key]
        public int Id { get; set; }
        public string codigo { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public bool Estado { get; set; }
    }
    
}