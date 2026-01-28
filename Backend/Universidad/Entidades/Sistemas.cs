using System.ComponentModel.DataAnnotations;

public class Profesor
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Nombre { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La categoría es requerida")]
    [StringLength(50, ErrorMessage = "La categoría no puede exceder 50 caracteres")]
    public string Categoria { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El correo es requerido")]
    [EmailAddress(ErrorMessage = "Formato de correo inválido")]
    public string Correo { get; set; } = string.Empty;
    
    [StringLength(200, ErrorMessage = "La especialidad no puede exceder 200 caracteres")]
    public string? Especialidad { get; set; }
    
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
}
