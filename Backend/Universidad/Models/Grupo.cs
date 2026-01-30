namespace Universidad.Models;

public class Grupo
{
    public int Id { get; set; }
    public int MateriaId { get; set; }
    public string Paralelo { get; set; } = "A";
    public string Turno { get; set; } = "Mañana";
    public int CupoMaximo { get; set; } = 30;
    public int CupoActual { get; set; } = 0;
    public int? DocenteId { get; set; }
    public string Aula { get; set; } = string.Empty;
    public string Horarios { get; set; } = "Lunes 8:00-10:00, Miércoles 8:00-10:00";
    public bool Activo { get; set; } = true;
    
    // Navigation properties
    public Materia Materia { get; set; } = null!;
    public Docente? Docente { get; set; }
    public List<Inscripcion> Inscripciones { get; set; } = new();
}
