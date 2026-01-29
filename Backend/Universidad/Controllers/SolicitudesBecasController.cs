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
    public class SolicitudesBecasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SolicitudesBecasController(AppDbContext context)
        {
            _context = context;
        }

        // 1. LISTAR TODAS LAS SOLICITUDES (GET: api/solicitudesbecas)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SolicitudBeca>>> GetSolicitudes()
        {
            return await _context.SolicitudesBecas.ToListAsync();
        }

        // 2. OBTENER UNA SOLICITUD POR ID (GET: api/solicitudesbecas/5)
        [HttpGet("{id}")]
        public async Task<ActionResult<SolicitudBeca>> GetSolicitud(int id)
        {
            var solicitud = await _context.SolicitudesBecas.FindAsync(id);

            if (solicitud == null)
            {
                return NotFound();
            }

            return solicitud;
        }

        // 3. CREAR NUEVA SOLICITUD (POST: api/solicitudesbecas)
        [HttpPost]
        public async Task<ActionResult<SolicitudBeca>> PostSolicitud(SolicitudBeca solicitud)
        {
            solicitud.FechaSolicitud = DateTime.Now;
            solicitud.Estado = "Pendiente"; // Estado inicial

            _context.SolicitudesBecas.Add(solicitud);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSolicitud), new { id = solicitud.Id }, solicitud);
        }

        // 4. ACTUALIZAR SOLICITUD (PUT: api/solicitudesbecas/5)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSolicitud(int id, SolicitudBeca solicitud)
        {
            if (id != solicitud.Id)
            {
                return BadRequest();
            }

            _context.Entry(solicitud).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolicitudExists(id))
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

        // 5. APROBAR/RECHAZAR SOLICITUD (PATCH: api/solicitudesbecas/5/estado)
        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] string nuevoEstado)
        {
            var solicitud = await _context.SolicitudesBecas.FindAsync(id);
            if (solicitud == null)
            {
                return NotFound();
            }

            solicitud.Estado = nuevoEstado;
            solicitud.FechaRespuesta = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 6. ELIMINAR SOLICITUD (DELETE: api/solicitudesbecas/5)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolicitud(int id)
        {
            var solicitud = await _context.SolicitudesBecas.FindAsync(id);
            if (solicitud == null)
            {
                return NotFound();
            }

            _context.SolicitudesBecas.Remove(solicitud);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SolicitudExists(int id)
        {
            return _context.SolicitudesBecas.Any(e => e.Id == id);
        }
    }
}
