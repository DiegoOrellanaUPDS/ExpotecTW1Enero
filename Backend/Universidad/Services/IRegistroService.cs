using Universidad.DTOs;
using Universidad.Models;

namespace Universidad.Services;

public interface IRegistroService
{
    // Estudiantes
    System.Threading.Tasks.Task<System.Collections.Generic.List<Estudiante>> GetEstudiantesAsync();
    System.Threading.Tasks.Task<Estudiante?> GetEstudianteByIdAsync(int id);
    
    // Inscripciones
    System.Threading.Tasks.Task<Inscripcion> InscribirEstudianteAsync(InscripcionDTO inscripcionDto);
    System.Threading.Tasks.Task<System.Collections.Generic.List<Inscripcion>> GetInscripcionesPorTurnoAsync(string turno);
    System.Threading.Tasks.Task<bool> CambiarGrupoAsync(int inscripcionId, CambioGrupoDTO cambioDto);
    
    // Proyecciones
    System.Threading.Tasks.Task<System.Collections.Generic.List<Materia>> GenerarProyeccionAsync(ProyeccionDTO proyeccionDto);
    
    // Grupos y Docentes
    System.Threading.Tasks.Task<Grupo> AsignarDocenteAGrupoAsync(AsignacionDocenteDTO asignacionDto);
    System.Threading.Tasks.Task<object> GenerarReporteCuposAsync(int materiaId);
    
    // Verano
    System.Threading.Tasks.Task<Inscripcion> InscribirVeranoAsync(InscripcionVeranoDTO veranoDto);
}
