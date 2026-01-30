using Universidad.DTOs;
using Universidad.Models;

namespace Universidad.Services;

public interface IRegistroService
{
    // Estudiantes
    Task<List<Estudiante>> GetEstudiantesAsync();
    Task<Estudiante?> GetEstudianteByIdAsync(int id);
    
    // Inscripciones
    Task<Inscripcion> InscribirEstudianteAsync(InscripcionDTO inscripcionDto);
    Task<List<Inscripcion>> GetInscripcionesPorTurnoAsync(string turno);
    Task<bool> CambiarGrupoAsync(int inscripcionId, CambioGrupoDTO cambioDto);
    
    // Proyecciones
    Task<List<Materia>> GenerarProyeccionAsync(ProyeccionDTO proyeccionDto);
    
    // Grupos y Docentes
    Task<Grupo> AsignarDocenteAGrupoAsync(AsignacionDocenteDTO asignacionDto);
    Task<object> GenerarReporteCuposAsync(int materiaId);
    
    // Verano
    Task<Inscripcion> InscribirVeranoAsync(InscripcionVeranoDTO veranoDto);
}
