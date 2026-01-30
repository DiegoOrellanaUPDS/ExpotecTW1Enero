using Microsoft.EntityFrameworkCore;
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

    public async Task<List<Estudiante>> GetEstudiantesAsync()
    {
        return await _context.Estudiantes
            .Where(e => e.Activo)
            .OrderBy(e => e.Codigo)
            .ToListAsync();
    }

    public async Task<Estudiante?> GetEstudianteByIdAsync(int id)
    {
        return await _context.Estudiantes
            .Include(e => e.Inscripciones)
            .FirstOrDefaultAsync(e => e.Id == id && e.Activo);
    }

    public async Task<Inscripcion> InscribirEstudianteAsync(InscripcionDTO inscripcionDto)
    {
        var grupo = await _context.Grupos.FindAsync(inscripcionDto.GrupoId);
        if (grupo == null || grupo.CupoActual >= grupo.CupoMaximo)
            throw new Exception("No hay cupos disponibles");

        var inscripcion = new Inscripcion
        {
            EstudianteId = inscripcionDto.EstudianteId,
            MateriaId = inscripcionDto.MateriaId,
            GrupoId = inscripcionDto.GrupoId,
            Turno = inscripcionDto.Turno,
            Estado = "Activa",
            EsVerano = inscripcionDto.EsVerano,
            FechaInscripcion = DateTime.Now
        };

        grupo.CupoActual++;
        _context.Inscripciones.Add(inscripcion);
        await _context.SaveChangesAsync();

        return inscripcion;
    }

    public async Task<List<Inscripcion>> GetInscripcionesPorTurnoAsync(string turno)
    {
        return await _context.Inscripciones
            .Include(i => i.Estudiante)
            .Include(i => i.Materia)
            .Include(i => i.Grupo)
            .Where(i => i.Turno == turno && i.Estado == "Activa")
            .ToListAsync();
    }

    public async Task<bool> CambiarGrupoAsync(int inscripcionId, CambioGrupoDTO cambioDto)
    {
        var inscripcion = await _context.Inscripciones.FindAsync(inscripcionId);
        if (inscripcion == null) return false;

        var nuevoGrupo = await _context.Grupos.FindAsync(cambioDto.NuevaGrupoId);
        if (nuevoGrupo == null || nuevoGrupo.CupoActual >= nuevoGrupo.CupoMaximo)
            return false;

        var grupoAntiguo = await _context.Grupos.FindAsync(inscripcion.GrupoId);
        if (grupoAntiguo != null)
            grupoAntiguo.CupoActual--;

        nuevoGrupo.CupoActual++;
        inscripcion.GrupoId = cambioDto.NuevaGrupoId;
        inscripcion.Turno = nuevoGrupo.Turno;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Materia>> GenerarProyeccionAsync(ProyeccionDTO proyeccionDto)
    {
        var materias = await _context.Materias
            .Where(m => m.Semestre == proyeccionDto.Semestre && m.Activa)
            .Take(5)
            .ToListAsync();

        var proyeccion = new Proyeccion
        {
            EstudianteId = proyeccionDto.EstudianteId,
            Semestre = proyeccionDto.Semestre,
            MateriasIds = string.Join(",", materias.Select(m => m.Id)),
            Estado = "Pendiente"
        };

        _context.Proyecciones.Add(proyeccion);
        await _context.SaveChangesAsync();

        return materias;
    }

    public async Task<Grupo> AsignarDocenteAGrupoAsync(AsignacionDocenteDTO asignacionDto)
    {
        var grupo = await _context.Grupos.FindAsync(asignacionDto.GrupoId);
        if (grupo == null)
            throw new Exception("Grupo no encontrado");

        grupo.DocenteId = asignacionDto.DocenteId;
        if (!string.IsNullOrEmpty(asignacionDto.Aula))
            grupo.Aula = asignacionDto.Aula;

        await _context.SaveChangesAsync();
        return grupo;
    }

    public async Task<object> GenerarReporteCuposAsync(int materiaId)
    {
        var materia = await _context.Materias
            .Include(m => m.Grupos)
                .ThenInclude(g => g.Docente)
            .FirstOrDefaultAsync(m => m.Id == materiaId);

        if (materia == null)
            throw new Exception("Materia no encontrada");

        return new
        {
            Materia = new { materia.Id, materia.Codigo, materia.Nombre },
            Grupos = materia.Grupos.Select(g => new
            {
                g.Paralelo,
                g.Turno,
                CuposDisponibles = g.CupoMaximo - g.CupoActual,
                CuposOcupados = g.CupoActual,
                Docente = g.Docente != null ? $"{g.Docente.Nombre} {g.Docente.Apellido}" : "Sin asignar",
                g.Aula
            })
        };
    }

    public async Task<Inscripcion> InscribirVeranoAsync(InscripcionVeranoDTO veranoDto)
    {
        var inscripcionDto = new InscripcionDTO
        {
            EstudianteId = veranoDto.EstudianteId,
            MateriaId = veranoDto.MateriaId,
            GrupoId = veranoDto.GrupoId,
            Turno = veranoDto.Turno,
            EsVerano = true
        };

        return await InscribirEstudianteAsync(inscripcionDto);
    }
}
