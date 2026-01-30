namespace Universidad.DTOs;

public class InscripcionDTO
{
    public int EstudianteId { get; set; }
    public int MateriaId { get; set; }
    public int GrupoId { get; set; }
    public string Turno { get; set; } = "Mañana";
    public bool EsVerano { get; set; } = false;
}

public class InscripcionVeranoDTO : InscripcionDTO
{
    public string Periodo { get; set; } = "Verano-2024";
}

public class CambioGrupoDTO
{
    public int NuevaGrupoId { get; set; }
    public string Motivo { get; set; } = string.Empty;
}

public class AsignacionDocenteDTO
{
    public int GrupoId { get; set; }
    public int DocenteId { get; set; }
    public string Turno { get; set; } = "Mañana";
    public string Aula { get; set; } = string.Empty;
}

public class ProyeccionDTO
{
    public int EstudianteId { get; set; }
    public int Semestre { get; set; }
    public string PreferenciasTurno { get; set; } = "Mañana,Tarde";
}
