namespace Universidad.Models;

public class Proyeccion
{
    public int Id { get; set; }
    public int EstudianteId { get; set; }
    public int Semestre { get; set; }
    public int Año { get; set; } = DateTime.Now.Year;
    public string MateriasIds { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    public string Estado { get; set; } = "Pendiente";
    public string Observaciones { get; set; } = string.Empty;
    
    // Navigation property
    public Estudiante Estudiante { get; set; } = null!;
}
