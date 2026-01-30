using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Universidad.Entidades;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/asistente/carreras")]
    public class CarreraAsistenteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CarreraAsistenteController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/asistente/carreras
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarreraAsistente>>> GetCarreras()
        {
            return await _context.CarrerasAsistente.ToListAsync();
        }

        // GET: api/asistente/carreras/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CarreraAsistente>> GetCarrera(int id)
        {
            var carrera = await _context.CarrerasAsistente.FindAsync(id);

            if (carrera == null)
                return NotFound(new { mensaje = "Carrera no encontrada" });

            return carrera;
        }

        // POST: api/asistente/carreras
        [HttpPost]
        public async Task<ActionResult<CarreraAsistente>> PostCarrera(CarreraAsistente carrera)
        {
            _context.CarrerasAsistente.Add(carrera);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCarrera", new { id = carrera.Id }, carrera);
        }

        // PUT: api/asistente/carreras/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarrera(int id, CarreraAsistente carrera)
        {
            if (id != carrera.Id)
                return BadRequest(new { mensaje = "ID no coincide" });

            _context.Entry(carrera).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarreraExists(id))
                    return NotFound(new { mensaje = "Carrera no encontrada" });
                else
                    throw;
            }

            return Ok(new { mensaje = "Carrera actualizada exitosamente" });
        }

        // DELETE: api/asistente/carreras/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarrera(int id)
        {
            var carrera = await _context.CarrerasAsistente.FindAsync(id);
            if (carrera == null)
                return NotFound(new { mensaje = "Carrera no encontrada" });

            _context.CarrerasAsistente.Remove(carrera);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Carrera eliminada exitosamente" });
        }

        // GET: api/asistente/carreras/activas
        [HttpGet("activas")]
        public async Task<ActionResult<IEnumerable<CarreraAsistente>>> GetCarrerasActivas()
        {
            return await _context.CarrerasAsistente
                .Where(c => c.Activa)
                .ToListAsync();
        }

        private bool CarreraExists(int id)
        {
            return _context.CarrerasAsistente.Any(e => e.Id == id);
        }
    }
}