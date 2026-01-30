using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Universidad.Entidades;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/asistente/solicitudes")]
    public class SolicitudAsistenteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SolicitudAsistenteController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/asistente/solicitudes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SolicitudAsistente>>> GetSolicitudes()
        {
            return await _context.SolicitudesAsistente.ToListAsync();
        }

        // GET: api/asistente/solicitudes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SolicitudAsistente>> GetSolicitud(int id)
        {
            var solicitud = await _context.SolicitudesAsistente.FindAsync(id);

            if (solicitud == null)
                return NotFound(new { mensaje = "Solicitud no encontrada" });

            return solicitud;
        }

        // POST: api/asistente/solicitudes
        [HttpPost]
        public async Task<ActionResult<SolicitudAsistente>> PostSolicitud(SolicitudAsistente solicitud)
        {
            solicitud.FechaSolicitud = DateTime.Now;
            solicitud.Estado = "Pendiente";
            
            _context.SolicitudesAsistente.Add(solicitud);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSolicitud", new { id = solicitud.Id }, solicitud);
        }

        // PUT: api/asistente/solicitudes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSolicitud(int id, SolicitudAsistente solicitud)
        {
            if (id != solicitud.Id)
                return BadRequest(new { mensaje = "ID no coincide" });

            _context.Entry(solicitud).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolicitudExists(id))
                    return NotFound(new { mensaje = "Solicitud no encontrada" });
                else
                    throw;
            }

            return Ok(new { mensaje = "Solicitud actualizada exitosamente" });
        }

        // DELETE: api/asistente/solicitudes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolicitud(int id)
        {
            var solicitud = await _context.SolicitudesAsistente.FindAsync(id);
            if (solicitud == null)
                return NotFound(new { mensaje = "Solicitud no encontrada" });

            _context.SolicitudesAsistente.Remove(solicitud);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Solicitud eliminada exitosamente" });
        }

        // GET: api/asistente/solicitudes/pendientes
        [HttpGet("pendientes")]
        public async Task<ActionResult<IEnumerable<SolicitudAsistente>>> GetSolicitudesPendientes()
        {
            return await _context.SolicitudesAsistente
                .Where(s => s.Estado == "Pendiente")
                .ToListAsync();
        }

        // PUT: api/asistente/solicitudes/{id}/aprobar
        [HttpPut("{id}/aprobar")]
        public async Task<IActionResult> AprobarSolicitud(int id)
        {
            var solicitud = await _context.SolicitudesAsistente.FindAsync(id);
            
            if (solicitud == null)
                return NotFound(new { mensaje = "Solicitud no encontrada" });

            solicitud.Estado = "Aprobada";
            solicitud.FechaRevision = DateTime.Now;
            
            _context.Entry(solicitud).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Solicitud aprobada exitosamente" });
        }

        // PUT: api/asistente/solicitudes/{id}/rechazar
        [HttpPut("{id}/rechazar")]
        public async Task<IActionResult> RechazarSolicitud(int id, [FromBody] string motivo)
        {
            var solicitud = await _context.SolicitudesAsistente.FindAsync(id);
            
            if (solicitud == null)
                return NotFound(new { mensaje = "Solicitud no encontrada" });

            solicitud.Estado = "Rechazada";
            solicitud.MotivoRechazo = motivo;
            solicitud.FechaRevision = DateTime.Now;
            
            _context.Entry(solicitud).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Solicitud rechazada exitosamente" });
        }

        private bool SolicitudExists(int id)
        {
            return _context.SolicitudesAsistente.Any(e => e.Id == id);
        }
    }
}