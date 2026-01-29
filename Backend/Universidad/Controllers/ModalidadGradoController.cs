using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Universidad.Entidades;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModalidadGradoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ModalidadGradoController(AppDbContext context)
        {
            _context = context;
        }

        // -------------------------------
        // GET: api/ModalidadGrado
        // -------------------------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModalidadGrado>>> GetModalidades()
        {
            return await _context.ModalidadesGrado.ToListAsync();
        }

        // -------------------------------
        // POST: api/ModalidadGrado
        // -------------------------------
        [HttpPost]
        public async Task<ActionResult<ModalidadGrado>> PostModalidad(ModalidadGrado modalidad)
        {
            var existe = await _context.ModalidadesGrado
                .FirstOrDefaultAsync(m => m.Nombre == modalidad.Nombre);

            if (existe != null)
                return BadRequest("Ya existe una modalidad con ese nombre.");

            await _context.ModalidadesGrado.AddAsync(modalidad);
            await _context.SaveChangesAsync();

            return Ok(modalidad);
        }

        // -------------------------------
        // PUT: api/ModalidadGrado/{id}
        // -------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModalidad(int id, ModalidadGrado modalidad)
        {
            var db = await _context.ModalidadesGrado.FirstOrDefaultAsync(m => m.Id == id);
            if (db == null)
                return NotFound("Modalidad no encontrada.");

            db.Nombre = modalidad.Nombre;
            db.Descripcion = modalidad.Descripcion;

            await _context.SaveChangesAsync();
            return Ok("Modalidad actualizada correctamente");
        }

        // -------------------------------
        // DELETE: api/ModalidadGrado/{id}
        // -------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModalidad(int id)
        {
            var db = await _context.ModalidadesGrado.FirstOrDefaultAsync(m => m.Id == id);
            if (db == null)
                return NotFound("Modalidad no encontrada.");

            _context.ModalidadesGrado.Remove(db);
            await _context.SaveChangesAsync();
            return Ok("Modalidad eliminada correctamente");
        }
    }
}
