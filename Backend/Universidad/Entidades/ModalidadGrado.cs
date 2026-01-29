using System.ComponentModel.DataAnnotations;

namespace Universidad.Entidades
{
    public class ModalidadGrado
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty; // inicializado para evitar CS8618

        [MaxLength(250)]
        public string Descripcion { get; set; } = string.Empty; // inicializado
    }
}
