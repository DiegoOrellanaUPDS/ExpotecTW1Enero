using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Universidad.Data;
using Universidad.DTOs;
using Universidad.Models;

namespace Universidad.Services;

public class RegistroService : IRegistroService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<RegistroService> _logger;

    public RegistroService(ApplicationDbContext context, ILogger<RegistroService> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Estudiantes
    public async System.Threading.Tasks.Task<System.Collections.Generic.List<Estudiante>> GetEstudiantesAsync()
    {
        return await _context.Estudiantes
            .Where(e => e.Activo)
            .OrderBy(e => e.Codigo)
            .ToListAsync();
    }

    public async System.Threading.Tasks.Task<Estudiante?> GetEstudianteByIdAsync(int id)
    {
        return await _context.Estudiantes
            .Include(e => e.Inscripciones)
            .FirstOrDefaultAsync(e => e.Id == id && e.Activo);
    }

    // Inscripciones
    public async System.Threading.Tasks.Task<Inscripcion> InscribirEstudianteAsync(InscripcionDTO inscripcionDto)
    {
        // Validar que el estudiante existe y está activo
        var estudiante = await _context.Estudiantes
            .FirstOrDefaultAsync(e => e.Id == inscripcionDto.EstudianteId && e.Activo);
        
        if (estudiante == null)
            throw new System.Exception("Estudiante no encontrado o inactivo");

        // Validar que la materia existe
        var materia = await _context.Materias
            .FirstOrDefaultAsync(m => m.Id == inscripcionDto.MateriaId && m.Activa);
        
        if (materia == null)
            throw new System.Exception("Materia no encontrada o inactiva");

        // Validar que el grupo existe y tiene cupos
        var grupo = await _context.Grupos
            .Include(g => g.Inscripciones)
            .FirstOrDefaultAsync(g => g.Id == inscripcionDto.GrupoId && g.Activo);
        
        if (grupo == null)
            throw new System.Exception("Grupo no encontrado o inactivo");
        
        if (grupo.CupoActual >= grupo.CupoMaximo)
            throw new System.Exception($"No hay cupos disponibles. Cupos ocupados: {grupo.CupoActual}/{grupo.CupoMaximo}");

        // Validar que no esté ya inscrito en esta materia
        var yaInscrito = await _context.Inscripciones
            .AnyAsync(i => i.EstudianteId == inscripcionDto.EstudianteId && 
                          i.MateriaId == inscripcionDto.MateriaId && 
                          i.Estado == "Activa");
        
        if (yaInscrito)
            throw new System.Exception("El estudiante ya está inscrito en esta materia");

        // Validar pre-requisitos (simplificado)
        if (!string.IsNullOrEmpty(materia.PreRequisitos))
        {
            var preRequisitos = materia.PreRequisitos.Split(',');
            var materiasAprobadas = await _context.Inscripciones
                .Where(i => i.EstudianteId == estudiante.Id && i.Estado == "Aprobada")
                .Select(i => i.MateriaId)
                .ToListAsync();
            
            foreach (var preReq in preRequisitos)
            {
                if (int.TryParse(preReq.Trim(), out int preReqId))
                {
                    if (!materiasAprobadas.Contains(preReqId))
                        throw new System.Exception($"No cumple con el pre-requisito: Materia ID {preReqId}");
                }
            }
        }

        // Crear la inscripción
        var inscripcion = new Inscripcion
        {
            EstudianteId = inscripcionDto.EstudianteId,
            MateriaId = inscripcionDto.MateriaId,
            GrupoId = inscripcionDto.GrupoId,
            Turno = inscripcionDto.Turno,
            Estado = "Activa",
            EsVerano = inscripcionDto.EsVerano,
            FechaInscripcion = System.DateTime.Now
        };

        // Actualizar cupos del grupo
        grupo.CupoActual++;

        // Guardar cambios
        _context.Inscripciones.Add(inscripcion);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Estudiante {estudiante.Codigo} inscrito en materia {materia.Codigo}, grupo {grupo.Paralelo}");
        
        return inscripcion;
    }

    public async System.Threading.Tasks.Task<System.Collections.Generic.List<Inscripcion>> GetInscripcionesPorTurnoAsync(string turno)
    {
        return await _context.Inscripciones
            .Include(i => i.Estudiante)
            .Include(i => i.Materia)
            .Include(i => i.Grupo)
            .Where(i => i.Turno == turno && i.Estado == "Activa")
            .OrderBy(i => i.FechaInscripcion)
            .ToListAsync();
    }

    public async System.Threading.Tasks.Task<bool> CambiarGrupoAsync(int inscripcionId, CambioGrupoDTO cambioDto)
    {
        var inscripcion = await _context.Inscripciones
            .Include(i => i.Grupo)
            .FirstOrDefaultAsync(i => i.Id == inscripcionId);
        
        if (inscripcion == null || inscripcion.Estado != "Activa")
            throw new System.Exception("Inscripción no encontrada o no está activa");

        var nuevoGrupo = await _context.Grupos
            .FirstOrDefaultAsync(g => g.Id == cambioDto.NuevaGrupoId && g.Activo);
        
        if (nuevoGrupo == null)
            throw new System.Exception("Grupo destino no encontrado o inactivo");
        
        if (nuevoGrupo.CupoActual >= nuevoGrupo.CupoMaximo)
            throw new System.Exception("No hay cupos disponibles en el grupo destino");

        // Validar que sea la misma materia
        if (inscripcion.MateriaId != nuevoGrupo.MateriaId)
            throw new System.Exception("No se puede cambiar a grupo de diferente materia");

        // Liberar cupo del grupo antiguo
        var grupoAntiguo = await _context.Grupos.FindAsync(inscripcion.GrupoId);
        if (grupoAntiguo != null && grupoAntiguo.CupoActual > 0)
        {
            grupoAntiguo.CupoActual--;
        }

        // Tomar cupo en el nuevo grupo
        nuevoGrupo.CupoActual++;

        // Actualizar la inscripción
        inscripcion.GrupoId = cambioDto.NuevaGrupoId;
        inscripcion.Turno = nuevoGrupo.Turno;

        await _context.SaveChangesAsync();
        
        _logger.LogInformation($"Inscripción {inscripcionId} cambiada del grupo {grupoAntiguo?.Paralelo} al grupo {nuevoGrupo.Paralelo}");
        
        return true;
    }

    // Proyecciones
    public async System.Threading.Tasks.Task<System.Collections.Generic.List<Materia>> GenerarProyeccionAsync(ProyeccionDTO proyeccionDto)
    {
        var estudiante = await GetEstudianteByIdAsync(proyeccionDto.EstudianteId);
        if (estudiante == null)
            throw new System.Exception("Estudiante no encontrado");

        // Obtener materias del semestre específico
        var materias = await _context.Materias
            .Where(m => m.Semestre == proyeccionDto.Semestre && m.Activa)
            .ToListAsync();

        // Filtrar por preferencias de turno
        var preferencias = proyeccionDto.PreferenciasTurno.Split(',');
        var materiasFiltradas = materias
            .Where(m => m.Grupos.Any(g => preferencias.Contains(g.Turno)))
            .Take(6) // Máximo 6 materias por proyección
            .ToList();

        // Crear registro de proyección
        var proyeccion = new Proyeccion
        {
            EstudianteId = proyeccionDto.EstudianteId,
            Semestre = proyeccionDto.Semestre,
            Año = System.DateTime.Now.Year,
            MateriasIds = string.Join(",", materiasFiltradas.Select(m => m.Id)),
            Estado = "Pendiente",
            Observaciones = $"Generada automáticamente el {System.DateTime.Now:dd/MM/yyyy}"
        };

        _context.Proyecciones.Add(proyeccion);
        await _context.SaveChangesAsync();

        return materiasFiltradas;
    }

    // Grupos y Docentes
    public async System.Threading.Tasks.Task<Grupo> AsignarDocenteAGrupoAsync(AsignacionDocenteDTO asignacionDto)
    {
        var grupo = await _context.Grupos
            .Include(g => g.Docente)
            .FirstOrDefaultAsync(g => g.Id == asignacionDto.GrupoId);
        
        if (grupo == null)
            throw new System.Exception("Grupo no encontrado");

        var docente = await _context.Docentes
            .FirstOrDefaultAsync(d => d.Id == asignacionDto.DocenteId && d.Activo);
        
        if (docente == null)
            throw new System.Exception("Docente no encontrado o inactivo");

        // Asignar docente
        grupo.DocenteId = asignacionDto.DocenteId;
        
        // Actualizar aula si se proporciona
        if (!string.IsNullOrEmpty(asignacionDto.Aula))
            grupo.Aula = asignacionDto.Aula;

        await _context.SaveChangesAsync();
        
        _logger.LogInformation($"Docente {docente.Codigo} asignado al grupo {grupo.Paralelo} de materia {grupo.MateriaId}");
        
        return grupo;
    }

    public async System.Threading.Tasks.Task<object> GenerarReporteCuposAsync(int materiaId)
    {
        var materia = await _context.Materias
            .Include(m => m.Grupos)
                .ThenInclude(g => g.Docente)
            .FirstOrDefaultAsync(m => m.Id == materiaId);
        
        if (materia == null)
            throw new System.Exception("Materia no encontrada");

        var reporte = new
        {
            Materia = new
            {
                materia.Id,
                materia.Codigo,
                materia.Nombre,
                materia.Semestre,
                materia.Creditos
            },
            Grupos = materia.Grupos
                .Where(g => g.Activo)
                .Select(g => new
                {
                    g.Id,
                    g.Paralelo,
                    g.Turno,
                    CuposDisponibles = g.CupoMaximo - g.CupoActual,
                    CuposOcupados = g.CupoActual,
                    CupoMaximo = g.CupoMaximo,
                    PorcentajeOcupacion = (g.CupoActual * 100) / g.CupoMaximo,
                    Docente = g.Docente != null ? $"{g.Docente.Nombre} {g.Docente.Apellido}" : "Sin asignar",
                    g.Aula,
                    g.Horarios
                })
                .OrderBy(g => g.Paralelo)
                .ThenBy(g => g.Turno)
                .ToList()
        };

        return reporte;
    }

    // Verano
    public async System.Threading.Tasks.Task<Inscripcion> InscribirVeranoAsync(InscripcionVeranoDTO veranoDto)
    {
        // Primero validar que el estudiante puede inscribirse en verano
        var estudiante = await GetEstudianteByIdAsync(veranoDto.EstudianteId);
        if (estudiante == null)
            throw new System.Exception("Estudiante no encontrado");

        // Validar promedio mínimo para verano (ejemplo: 70/100)
        if (estudiante.Promedio < 70)
            throw new System.Exception($"Promedio insuficiente para verano. Promedio actual: {estudiante.Promedio}");

        // Usar el método de inscripción normal pero marcando como verano
        var inscripcionDto = new InscripcionDTO
        {
            EstudianteId = veranoDto.EstudianteId,
            MateriaId = veranoDto.MateriaId,
            GrupoId = veranoDto.GrupoId,
            Turno = veranoDto.Turno,
            EsVerano = true
        };

        var inscripcion = await InscribirEstudianteAsync(inscripcionDto);
        
        // Marcar adicionalmente como verano en observaciones (si fuera necesario)
        _logger.LogInformation($"Estudiante {estudiante.Codigo} inscrito en verano para materia {veranoDto.MateriaId}, periodo {veranoDto.Periodo}");
        
        return inscripcion;
    }
}
