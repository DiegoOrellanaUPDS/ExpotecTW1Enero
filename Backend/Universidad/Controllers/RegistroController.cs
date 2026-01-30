using Microsoft.AspNetCore.Mvc;
using Universidad.DTOs;
using Universidad.Services;

namespace Universidad.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegistroController : ControllerBase
{
    private readonly IRegistroService _registroService;
    private readonly ILogger<RegistroController> _logger;

    public RegistroController(IRegistroService registroService, ILogger<RegistroController> logger)
    {
        _registroService = registroService;
        _logger = logger;
    }

    [HttpGet("estudiantes")]
    public async Task<IActionResult> GetEstudiantes()
    {
        try
        {
            var estudiantes = await _registroService.GetEstudiantesAsync();
            return Ok(estudiantes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener estudiantes");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("estudiantes/{id}")]
    public async Task<IActionResult> GetEstudiante(int id)
    {
        try
        {
            var estudiante = await _registroService.GetEstudianteByIdAsync(id);
            return Ok(estudiante);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener estudiante {id}");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("inscribir")]
    public async Task<IActionResult> InscribirEstudiante([FromBody] InscripcionDTO inscripcionDto)
    {
        try
        {
            var resultado = await _registroService.InscribirEstudianteAsync(inscripcionDto);
            return Ok(new { message = "Inscripción exitosa", data = resultado });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al inscribir estudiante");
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("inscripciones/turno/{turno}")]
    public async Task<IActionResult> GetInscripcionesPorTurno(string turno)
    {
        try
        {
            var inscripciones = await _registroService.GetInscripcionesPorTurnoAsync(turno);
            return Ok(new { turno, inscripciones });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener inscripciones para turno {turno}");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPut("cambiar-grupo/{inscripcionId}")]
    public async Task<IActionResult> CambiarGrupo(int inscripcionId, [FromBody] CambioGrupoDTO cambioDto)
    {
        try
        {
            var resultado = await _registroService.CambiarGrupoAsync(inscripcionId, cambioDto);
            return Ok(new { message = "Grupo cambiado exitosamente", resultado });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al cambiar grupo para inscripción {inscripcionId}");
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("proyeccion/generar")]
    public async Task<IActionResult> GenerarProyeccion([FromBody] ProyeccionDTO proyeccionDto)
    {
        try
        {
            var materias = await _registroService.GenerarProyeccionAsync(proyeccionDto);
            return Ok(new { message = "Proyección generada", materias });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al generar proyección");
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("asignar-docente")]
    public async Task<IActionResult> AsignarDocente([FromBody] AsignacionDocenteDTO asignacionDto)
    {
        try
        {
            var resultado = await _registroService.AsignarDocenteAGrupoAsync(asignacionDto);
            return Ok(new { message = "Docente asignado", resultado });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al asignar docente");
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("reporte/cupos/{materiaId}")]
    public async Task<IActionResult> GetReporteCupos(int materiaId)
    {
        try
        {
            var reporte = await _registroService.GenerarReporteCuposAsync(materiaId);
            return Ok(reporte);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al generar reporte de cupos");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("verano/inscribir")]
    public async Task<IActionResult> InscribirVerano([FromBody] InscripcionVeranoDTO veranoDto)
    {
        try
        {
            var resultado = await _registroService.InscribirVeranoAsync(veranoDto);
            return Ok(new { message = "Inscripción a verano exitosa", resultado });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inscribir en verano");
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { status = "OK", service = "Registro API", timestamp = DateTime.Now });
    }
}
