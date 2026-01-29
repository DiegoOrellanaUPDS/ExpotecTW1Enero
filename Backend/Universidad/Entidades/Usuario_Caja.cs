using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entidades
{
    [Index(nameof(Id), IsUnique = true)]
    public class Usuario_Caja
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; } = "-1";
        [NotMapped]
        public string? Password { get; set; }
        public string Rol { get; set; } = "user";
        public string Token { get; set; } = "-1";
        public string Estado { get; set; } = "Activo";

    }
}
