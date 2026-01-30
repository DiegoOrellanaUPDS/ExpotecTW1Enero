using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Universidad.Entidades;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/asistente/materias")]
    public class MateriaAsistenteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MateriaAsistenteController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/asistente/materias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MateriaAsistente>>> GetMaterias()
        {
            return await _context.MateriasAsistente.ToListAsync();
        }

        // GET: api/asistente/materias/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MateriaAsistente>> GetMateria(int id)
        {
            var materia = await _context.MateriasAsistente.FindAsync(id);

            if (materia == null)
                return NotFound(new { mensaje = "Materia no encontrada" });

            return materia;
        }

        // POST: api/asistente/materias
        [HttpPost]
        public async Task<ActionResult<MateriaAsistente>> PostMateria(MateriaAsistente materia)
        {
            _context.MateriasAsistente.Add(materia);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMateria", new { id = materia.Id }, materia);
        }

        // PUT: api/asistente/materias/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMateria(int id, MateriaAsistente materia)
        {
            if (id != materia.Id)
                return BadRequest(new { mensaje = "ID no coincide" });

            _context.Entry(materia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MateriaExists(id))
                    return NotFound(new { mensaje = "Materia no encontrada" });
                else
                    throw;
            }

            return Ok(new { mensaje = "Materia actualizada exitosamente" });
        }

        // DELETE: api/asistente/materias/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMateria(int id)
        {
            var materia = await _context.MateriasAsistente.FindAsync(id);
            if (materia == null)
                return NotFound(new { mensaje = "Materia no encontrada" });

            _context.MateriasAsistente.Remove(materia);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Materia eliminada exitosamente" });
        }

        // GET: api/asistente/materias/activas
        [HttpGet("activas")]
        public async Task<ActionResult<IEnumerable<MateriaAsistente>>> GetMateriasActivas()
        {
            return await _context.MateriasAsistente
                .Where(m => m.Activa)
                .ToListAsync();
        }

        // GET: api/asistente/materias/carrera/{carreraId}
        [HttpGet("carrera/{carreraId}")]
        public async Task<ActionResult<IEnumerable<MateriaAsistente>>> GetMateriasPorCarrera(int carreraId)
        {
            return await _context.MateriasAsistente
                .Where(m => m.CarreraId == carreraId)
                .ToListAsync();
        }

        private bool MateriaExists(int id)
        {
            return _context.MateriasAsistente.Any(e => e.Id == id);
        }
    }
}