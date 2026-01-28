using System.ComponentModel.DataAnnotations;

namespace Universidad.Entidades
{
    public class LimpiezaInsumo
    {
        [Key]
        public int Id { get; set; }

        public string Producto { get; set; } = string.Empty; // Ej: Jabón Líquido

        public int Cantidad { get; set; } // Ej: 50 litros

        public string UbicacionGuardado { get; set; } = string.Empty; // Ej: Depósito A
    }
}