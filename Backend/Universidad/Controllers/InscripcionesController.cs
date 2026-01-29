using Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Data;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InscripcionesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public InscripcionesController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var lista = await _db.Inscripciones
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return Ok(lista);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var item = await _db.Inscripciones.FindAsync(id);
            if (item == null) return NotFound(new { mensaje = "Inscripción no encontrada" });
            return Ok(item);
        }

        [HttpGet("por-estudiante/{estudianteId:int}")]
        public async Task<IActionResult> PorEstudiante(int estudianteId)
        {
            var lista = await _db.Inscripciones
                .Where(x => x.EstudianteId == estudianteId)
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return Ok(lista);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearInscripcionDto dto)
        {
            if (dto.EstudianteId <= 0 || dto.MateriaId <= 0)
                return BadRequest(new { mensaje = "EstudianteId y MateriaId deben ser > 0" });

            if (string.IsNullOrWhiteSpace(dto.Periodo))
                return BadRequest(new { mensaje = "Periodo es obligatorio" });

            dto.Periodo = dto.Periodo.Trim().ToUpper();

            var existe = await _db.Inscripciones.AnyAsync(x =>
                x.EstudianteId == dto.EstudianteId &&
                x.MateriaId == dto.MateriaId &&
                x.Periodo == dto.Periodo &&
                x.Estado == "ACTIVA"
            );

            if (existe)
                return BadRequest(new { mensaje = "Ya existe una inscripción ACTIVA para ese estudiante/materia/periodo." });

            // ✅ Discord: lo más seguro es NameIdentifier y Email
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized(new { mensaje = "No se pudo obtener el identificador del usuario autenticado." });

            var ins = new Inscripcion
            {
                EstudianteId = dto.EstudianteId,
                MateriaId = dto.MateriaId,
                Periodo = dto.Periodo,
                FechaInscripcion = DateTime.UtcNow,
                Estado = "ACTIVA",
                UsuarioId = userId,
                UsuarioEmail = email
            };

            _db.Inscripciones.Add(ins);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(Obtener), new { id = ins.Id }, ins);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] CambiarEstadoDto dto)
        {
            var ins = await _db.Inscripciones.FindAsync(id);
            if (ins == null) return NotFound(new { mensaje = "Inscripción no encontrada" });

            var estado = (dto.Estado ?? "").Trim().ToUpper();
            if (estado != "ACTIVA" && estado != "ANULADA" && estado != "INACTIVA")
                return BadRequest(new { mensaje = "Estado inválido. Usa: ACTIVA, ANULADA o INACTIVA." });

            ins.Estado = estado;
            await _db.SaveChangesAsync();

            return Ok(ins);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var ins = await _db.Inscripciones.FindAsync(id);
            if (ins == null) return NotFound(new { mensaje = "Inscripción no encontrada" });

            _db.Inscripciones.Remove(ins);
            await _db.SaveChangesAsync();

            return Ok(new { mensaje = "Inscripción eliminada" });
        }
    }

    public class CrearInscripcionDto
    {
        public int EstudianteId { get; set; }
        public int MateriaId { get; set; }
        public string Periodo { get; set; } = "";
    }

    public class CambiarEstadoDto
    {
        public string Estado { get; set; } = "ACTIVA";
    }
}
