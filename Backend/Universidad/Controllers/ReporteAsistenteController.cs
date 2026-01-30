using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Universidad.Entidades;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/asistente/reportes")]
    public class ReporteAsistenteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReporteAsistenteController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/asistente/reportes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReporteAsistente>>> GetReportes()
        {
            return await _context.ReportesAsistente
                .OrderByDescending(r => r.FechaGeneracion)
                .ToListAsync();
        }

        // GET: api/asistente/reportes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ReporteAsistente>> GetReporte(int id)
        {
            var reporte = await _context.ReportesAsistente.FindAsync(id);

            if (reporte == null)
                return NotFound(new { mensaje = "Reporte no encontrado" });

            return reporte;
        }

        // POST: api/asistente/reportes
        [HttpPost]
        public async Task<ActionResult<ReporteAsistente>> PostReporte(ReporteAsistente reporte)
        {
            reporte.FechaGeneracion = DateTime.Now;
            
            _context.ReportesAsistente.Add(reporte);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReporte", new { id = reporte.Id }, reporte);
        }

        // PUT: api/asistente/reportes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReporte(int id, ReporteAsistente reporte)
        {
            if (id != reporte.Id)
                return BadRequest(new { mensaje = "ID no coincide" });

            _context.Entry(reporte).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReporteExists(id))
                    return NotFound(new { mensaje = "Reporte no encontrado" });
                else
                    throw;
            }

            return Ok(new { mensaje = "Reporte actualizado exitosamente" });
        }

        // DELETE: api/asistente/reportes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReporte(int id)
        {
            var reporte = await _context.ReportesAsistente.FindAsync(id);
            if (reporte == null)
                return NotFound(new { mensaje = "Reporte no encontrado" });

            _context.ReportesAsistente.Remove(reporte);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Reporte eliminado exitosamente" });
        }

        // GET: api/asistente/reportes/tipo/{tipo}
        [HttpGet("tipo/{tipo}")]
        public async Task<ActionResult<IEnumerable<ReporteAsistente>>> GetReportesPorTipo(string tipo)
        {
            return await _context.ReportesAsistente
                .Where(r => r.Tipo == tipo)
                .OrderByDescending(r => r.FechaGeneracion)
                .ToListAsync();
        }

        // GET: api/asistente/reportes/periodo/{periodo}
        [HttpGet("periodo/{periodo}")]
        public async Task<ActionResult<IEnumerable<ReporteAsistente>>> GetReportesPorPeriodo(string periodo)
        {
            return await _context.ReportesAsistente
                .Where(r => r.Periodo == periodo)
                .OrderByDescending(r => r.FechaGeneracion)
                .ToListAsync();
        }

        // GET: api/asistente/reportes/estadisticas
        [HttpGet("estadisticas")]
        public async Task<ActionResult<object>> GetEstadisticasReportes()
        {
            var reportes = await _context.ReportesAsistente.ToListAsync();
            
            var estadisticas = new
            {
                totalReportes = reportes.Count,
                promedioGeneral = reportes.Any() ? reportes.Average(r => r.PromedioGeneral) : 0,
                promedioDesercion = reportes.Any() ? reportes.Average(r => r.PorcentajeDesercion) : 0,
                porTipo = reportes
                    .GroupBy(r => r.Tipo)
                    .Select(g => new
                    {
                        tipo = g.Key,
                        cantidad = g.Count(),
                        ultimoPeriodo = g.Max(r => r.Periodo)
                    })
                    .ToList()
            };

            return Ok(estadisticas);
        }

        private bool ReporteExists(int id)
        {
            return _context.ReportesAsistente.Any(e => e.Id == id);
        }
    }
}