using System.ComponentModel.DataAnnotations;

namespace Universidad.Entidades
{
    public class ObjetoPerdido
    {
        [Key]
        public int Id { get; set; }

        public string Aula { get; set; } = string.Empty; // Ej: Aula 102

        public string TipoObjeto { get; set; } = string.Empty; // Ej: Cargador de Laptop

        public string Descripcion { get; set; } = string.Empty; // Ej: Blanco marca Apple

        public DateTime FechaEncontrado { get; set; } = DateTime.UtcNow;

        public bool EntregadoADueño { get; set; } = false;
    }
}