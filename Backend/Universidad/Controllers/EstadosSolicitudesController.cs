using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Data;
using Entidades;
using Universidad.Entidades;

namespace Universidad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class EstadosSolicitudesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EstadosSolicitudesController(AppDbContext context)
        {
            _context = context;
        }

        // 1. LISTAR TODOS LOS ESTADOS (GET: api/estadossolicitudes)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoSolicitud>>> GetEstados()
        {
            return await _context.EstadosSolicitudes.ToListAsync();
        }

        // 2. OBTENER UN ESTADO POR ID (GET: api/estadossolicitudes/5)
        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoSolicitud>> GetEstado(int id)
        {
            var estado = await _context.EstadosSolicitudes.FindAsync(id);

            if (estado == null)
            {
                return NotFound();
            }

            return estado;
        }

        // 3. CREAR NUEVO ESTADO (POST: api/estadossolicitudes)
        [HttpPost]
        public async Task<ActionResult<EstadoSolicitud>> PostEstado(EstadoSolicitud estado)
        {
            _context.EstadosSolicitudes.Add(estado);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEstado), new { id = estado.Id }, estado);
        }

        // 4. ACTUALIZAR ESTADO (PUT: api/estadossolicitudes/5)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstado(int id, EstadoSolicitud estado)
        {
            if (id != estado.Id)
            {
                return BadRequest();
            }

            _context.Entry(estado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // 5. ELIMINAR ESTADO (DELETE: api/estadossolicitudes/5)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstado(int id)
        {
            var estado = await _context.EstadosSolicitudes.FindAsync(id);
            if (estado == null)
            {
                return NotFound();
            }

            _context.EstadosSolicitudes.Remove(estado);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstadoExists(int id)
        {
            return _context.EstadosSolicitudes.Any(e => e.Id == id);
        }
    }
}
