namespace Universidad.Models;

public class Docente
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Contrato { get; set; } = "Tiempo Completo";
    public string Especialidad { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    
    // Navigation properties
    public List<Grupo> GruposAsignados { get; set; } = new();
}
