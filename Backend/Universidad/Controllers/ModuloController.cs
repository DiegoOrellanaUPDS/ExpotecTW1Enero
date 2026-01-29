using Data;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModulosController : ControllerBase
    {
        private AppDbContext context;

        public ModulosController(AppDbContext context)
        {
            this.context = context;
        }

        // GET: api/Modulos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Modulo>>> GetModulos()
        {
            return Ok(await context.Modulos
                .Where(m => m.Estado != "Borrado")
                .ToListAsync());
        }

        // GET: api/Modulos/{codigo}
        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetModulo(string codigo)
        {
            var modulo = await context.Modulos
                .Where(m => m.Codigo == codigo && m.Estado != "Borrado")
                .FirstOrDefaultAsync();

            if (modulo == null)
                return NotFound();

            return Ok(modulo);
        }

        // POST: api/Modulos
        [HttpPost]
        public async Task<IActionResult> CreateModulo(Modulo modulo)
        {
            var existe = await context.Modulos
                .Where(m => m.Codigo == modulo.Codigo && m.Estado != "Borrado")
                .FirstOrDefaultAsync();

            if (existe != null)
                return BadRequest("El módulo ya existe.");

            modulo.Estado = "Activo";

            await context.Modulos.AddAsync(modulo);
            await context.SaveChangesAsync();

            return Ok(modulo);
        }

        // PUT: api/Modulos/{codigo}
        [HttpPut("{codigo}")]
        public async Task<IActionResult> UpdateModulo(
            string codigo,
            [FromBody] Modulo modulo
        )
        {
            var existing = await context.Modulos
                .Where(m => m.Codigo == codigo && m.Estado != "Borrado")
                .FirstOrDefaultAsync();

            if (existing == null)
                return NotFound();

            existing.NumeroModulo = modulo.NumeroModulo;
            existing.Anio = modulo.Anio;
            existing.Mes = modulo.Mes;
            existing.Turno = modulo.Turno;
            existing.FechaInicio = modulo.FechaInicio;
            existing.FechaFin = modulo.FechaFin;
            existing.LlaveForaneaIdMateria = modulo.LlaveForaneaIdMateria;
            existing.LlaveForaneaIdSemestre = modulo.LlaveForaneaIdSemestre;

            await context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Modulos/{codigo}
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> DeleteModulo(string codigo)
        {
            var modulo = await context.Modulos
                .Where(m => m.Codigo == codigo && m.Estado != "Borrado")
                .FirstOrDefaultAsync();

            if (modulo == null)
                return NotFound();

            modulo.Estado = "Borrado";
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
