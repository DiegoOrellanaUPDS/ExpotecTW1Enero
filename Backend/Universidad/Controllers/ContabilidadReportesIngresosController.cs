using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Universidad.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Entidades;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContabilidadReportesIngresosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContabilidadReportesIngresosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ContabilidadReportesIngresos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContabilidadReportesIngresos>>> GetAll()
        {
            return await _context.ContabilidadReportesIngresos.ToListAsync();
        }

        // GET: api/ContabilidadReportesIngresos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ContabilidadReportesIngresos>> GetById(int id)
        {
            var reporte = await _context.ContabilidadReportesIngresos.FindAsync(id);
            if (reporte == null) return NotFound();
            return reporte;
        }

        // POST: api/ContabilidadReportesIngresos
        [HttpPost]
        public async Task<ActionResult<ContabilidadReportesIngresos>> Create([FromBody] ContabilidadReportesIngresos nuevoReporte)
        {
            nuevoReporte.BalanceContabilidad = nuevoReporte.TotalIngresoContabilidad - nuevoReporte.TotalEgresosContabilidad;
            nuevoReporte.FechaContabilidad = DateTime.UtcNow;
            
            _context.ContabilidadReportesIngresos.Add(nuevoReporte);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetById), 
                new { id = nuevoReporte.IdReporteContabilidad }, 
                nuevoReporte);
        }

        // PUT: api/ContabilidadReportesIngresos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ContabilidadReportesIngresos reporteActualizado)
        {
            if (id != reporteActualizado.IdReporteContabilidad) return BadRequest();
            
            reporteActualizado.BalanceContabilidad = reporteActualizado.TotalIngresoContabilidad - reporteActualizado.TotalEgresosContabilidad;
            
            _context.Entry(reporteActualizado).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReporteExists(id)) return NotFound();
                else throw;
            }
            
            return NoContent();
        }

        // DELETE: api/ContabilidadReportesIngresos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var reporte = await _context.ContabilidadReportesIngresos.FindAsync(id);
            if (reporte == null) return NotFound();

            _context.ContabilidadReportesIngresos.Remove(reporte);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ENDPOINT EXTRA: Obtener reporte con cálculo automático
        [HttpPost("calcular")]
        public IActionResult CalcularReporte([FromBody] CalculoReporteRequest request)
        {
            var balance = request.Ingresos - request.Egresos;
            
            return Ok(new 
            {
                Ingresos = request.Ingresos,
                Egresos = request.Egresos,
                Balance = balance,
                Fecha = DateTime.UtcNow
            });
        }

        private bool ReporteExists(int id)
        {
            return _context.ContabilidadReportesIngresos.Any(e => e.IdReporteContabilidad == id);
        }
    }

    public class CalculoReporteRequest
    {
        public decimal Ingresos { get; set; }
        public decimal Egresos { get; set; }
    }
}