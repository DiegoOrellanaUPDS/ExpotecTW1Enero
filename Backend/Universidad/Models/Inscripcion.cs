namespace Universidad.Models;

public class Inscripcion
{
    public int Id { get; set; }
    public int EstudianteId { get; set; }
    public int MateriaId { get; set; }
    public int GrupoId { get; set; }
    public string Turno { get; set; } = "Mañana";
    public DateTime FechaInscripcion { get; set; } = DateTime.Now;
    public string Estado { get; set; } = "Activa";
    public bool EsVerano { get; set; } = false;
    
    // Navigation properties
    public Estudiante Estudiante { get; set; } = null!;
    public Materia Materia { get; set; } = null!;
    public Grupo Grupo { get; set; } = null!;
}
