using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    [Index(nameof(codigo), IsUnique = true)]
    public class Producto
    {
        [Key]
        public int idProducto { get; set; }
        /*FK*/
        public string codigo { get; set; } = null!;
        /*FK*/
        public string nombre { get; set; } = null!;
        public decimal precio_unitario { get; set; }
        public bool estado { get; set; } = true;
    }
}