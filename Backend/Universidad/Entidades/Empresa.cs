using System.ComponentModel.DataAnnotations;

namespace Universidad.Entidades
{
    public class Empresa
    {
        [Key]
        public int Id { get; set; }
        public string NIT { get; set; } // Valor para futuras referencias
        public string Nombre { get; set; }
        public string RazonSocial { get; set; } //nombre legal de la empresa
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public bool Estado { get; set; }
    }
}