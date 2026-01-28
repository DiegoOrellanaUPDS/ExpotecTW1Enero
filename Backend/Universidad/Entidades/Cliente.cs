using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    [Index(nameof(codigo), IsUnique = true)]
    public class Cliente
    {
        [Key]
        public int idCliente { get; set; }
        public string codigo { get; set; } = null!;
        /*FK*/
        public string? ci { get; set; }
        public bool Estado { get; set; } = true;
    }
}