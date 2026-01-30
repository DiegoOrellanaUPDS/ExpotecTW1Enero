using Microsoft.AspNetCore.Mvc;
using Universidad.DTOs;
using Universidad.Services;

namespace Universidad.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegistroController : ControllerBase
{
    private readonly IRegistroService _registroService;
    private readonly Microsoft.Extensions.Logging.ILogger<RegistroController> _logger;

    public RegistroController(IRegistroService registroService, Microsoft.Extensions.Logging.ILogger<RegistroController> logger)
    {
        _registroService = registroService;
        _logger = logger;
    }

    // GET: api/registro/estudiantes
    [HttpGet("estudiantes")]
    public async System.Threading.Tasks.Task<IActionResult> GetEstudiantes()
    {
        try
        {
            var estudiantes = await _registroService.GetEstudiantesAsync();
            return Ok(estudiantes);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error al obtener estudiantes");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    // GET: api/registro/estudiantes/{id}
    [HttpGet("estudiantes/{id}")]
    public async System.Threading.Tasks.Task<IActionResult> GetEstudiante(int id)
    {
        try
        {
            var estudiante = await _registroService.GetEstudianteByIdAsync(id);
            if (estudiante == null)
                return NotFound(new { message = $"Estudiante con ID {id} no encontrado" });
            
            return Ok(estudiante);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener estudiante {id}");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    // POST: api/registro/inscribir
    [HttpPost("inscribir")]
    public async System.Threading.Tasks.Task<IActionResult> InscribirEstudiante([FromBody] InscripcionDTO inscripcionDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _registroService.InscribirEstudianteAsync(inscripcionDto);
            return CreatedAtAction(nameof(GetEstudiante), new { id = resultado.EstudianteId }, new 
            { 
                message = "Inscripción exitosa", 
                data = resultado 
            });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error al inscribir estudiante");
            return BadRequest(new { error = ex.Message });
        }
    }

    // GET: api/registro/inscripciones/turno/{turno}
    [HttpGet("inscripciones/turno/{turno}")]
    public async System.Threading.Tasks.Task<IActionResult> GetInscripcionesPorTurno(string turno)
    {
        try
        {
            var inscripciones = await _registroService.GetInscripcionesPorTurnoAsync(turno);
            return Ok(new
            {
                turno,
                total = inscripciones.Count,
                inscripciones
            });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener inscripciones para turno {turno}");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    // PUT: api/registro/cambiar-grupo/{inscripcionId}
    [HttpPut("cambiar-grupo/{inscripcionId}")]
    public async System.Threading.Tasks.Task<IActionResult> CambiarGrupo(int inscripcionId, [FromBody] CambioGrupoDTO cambioDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _registroService.CambiarGrupoAsync(inscripcionId, cambioDto);
            return Ok(new 
            { 
                message = "Grupo cambiado exitosamente", 
                inscripcionId,
                nuevoGrupoId = cambioDto.NuevaGrupoId
            });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, $"Error al cambiar grupo para inscripción {inscripcionId}");
            return BadRequest(new { error = ex.Message });
        }
    }

    // POST: api/registro/proyeccion/generar
    [HttpPost("proyeccion/generar")]
    public async System.Threading.Tasks.Task<IActionResult> GenerarProyeccion([FromBody] ProyeccionDTO proyeccionDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var materias = await _registroService.GenerarProyeccionAsync(proyeccionDto);
            return Ok(new
            {
                message = "Proyección generada exitosamente",
                estudianteId = proyeccionDto.EstudianteId,
                semestre = proyeccionDto.Semestre,
                totalMaterias = materias.Count,
                materias
            });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, $"Error al generar proyección para estudiante {proyeccionDto.EstudianteId}");
            return BadRequest(new { error = ex.Message });
        }
    }

    // POST: api/registro/asignar-docente
    [HttpPost("asignar-docente")]
    public async System.Threading.Tasks.Task<IActionResult> AsignarDocente([FromBody] AsignacionDocenteDTO asignacionDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _registroService.AsignarDocenteAGrupoAsync(asignacionDto);
            return Ok(new 
            { 
                message = "Docente asignado exitosamente", 
                grupoId = resultado.Id,
                docenteId = resultado.DocenteId,
                paralelo = resultado.Paralelo
            });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, $"Error al asignar docente al grupo {asignacionDto.GrupoId}");
            return BadRequest(new { error = ex.Message });
        }
    }

    // GET: api/registro/reporte/cupos/{materiaId}
    [HttpGet("reporte/cupos/{materiaId}")]
    public async System.Threading.Tasks.Task<IActionResult> GetReporteCupos(int materiaId)
    {
        try
        {
            var reporte = await _registroService.GenerarReporteCuposAsync(materiaId);
            return Ok(reporte);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, $"Error al generar reporte de cupos para materia {materiaId}");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    // POST: api/registro/verano/inscribir
    [HttpPost("verano/inscribir")]
    public async System.Threading.Tasks.Task<IActionResult> InscribirVerano([FromBody] InscripcionVeranoDTO veranoDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _registroService.InscribirVeranoAsync(veranoDto);
            return Ok(new 
            { 
                message = "Inscripción a verano exitosa", 
                data = resultado,
                periodo = veranoDto.Periodo
            });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, $"Error al inscribir en verano al estudiante {veranoDto.EstudianteId}");
            return BadRequest(new { error = ex.Message });
        }
    }

    // Health check
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new 
        { 
            status = "OK", 
            timestamp = System.DateTime.Now,
            service = "Registro API",
            version = "1.0.0"
        });
    }
}
