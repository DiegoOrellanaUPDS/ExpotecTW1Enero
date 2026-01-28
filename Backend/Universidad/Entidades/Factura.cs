using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    [Index(nameof(codigo), IsUnique = true)]
    public class Factura
    {
        [Key]
        public int nroFactura { get; set; }
        public string codigo { get; set; } = null!;
        /*FK*/
        public string codigo_producto { get; set; } = null!;
        /*FK*/
        public string codigo_cliente { get; set; } = null!; //Sólo hasta tener tabla Persona
        /*FK*/
        public string? codigo_beca { get; set; }
        /*FK*/
        public int? nit_receptor { get; set; }
        /*Constante*/
        public int nit_emisor { get; set; } = 11111111;
        public int cantidad { get; set; } = 1;
        public string descripcion { get; set; } = null!;
        public decimal subtotal { get; set; } = 1;
        public decimal descuento { get; set; } = 0;
        public decimal monto_final { get; set; } = 0;
        public DateTime fecha_emision { get; set; }
        public bool Estado { get; set; } = true;
    }
}