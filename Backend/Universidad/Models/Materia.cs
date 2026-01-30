namespace Universidad.Models;

public class Materia
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Sigla { get; set; } = string.Empty;
    public int Creditos { get; set; } = 3;
    public int Semestre { get; set; } = 1;
    public string Tipo { get; set; } = "Obligatoria";
    public string Area { get; set; } = string.Empty;
    public bool Activa { get; set; } = true;
    public string PreRequisitos { get; set; } = string.Empty;
    
    // Navigation properties
    public List<Grupo> Grupos { get; set; } = new();
}
