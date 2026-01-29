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
    public class SemestresController : ControllerBase
    {
        private AppDbContext context;

        public SemestresController(AppDbContext context)
        {
            this.context = context;
        }

        // GET: api/Semestres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Semestre>>> GetSemestres()
        {
            return Ok(await context.Semestres
                .Where(s => s.Estado != "Borrado")
                .ToListAsync());
        }
        // GET: api/Semestres/{codigo}
        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetSemestre(string codigo)
        {
            var semestre = await (from sem in context.Semestres
                                  where sem.Codigo == codigo
                                  && sem.Estado != "Borrado"
                                  select sem).FirstOrDefaultAsync();

            if (semestre == null)
                return NotFound();

            return Ok(semestre);
        }

        // POST: api/Semestres
        [HttpPost]
        public async Task<IActionResult> CreateSemestre(Semestre semestre)
        {
            var existe = await (from sem in context.Semestres
                                where sem.Codigo == semestre.Codigo
                                && sem.Estado != "Borrado"
                                select sem).FirstOrDefaultAsync();

            if (existe != null)
                return BadRequest("El semestre ya existe.");

            semestre.Estado = "Activo";

            await context.Semestres.AddAsync(semestre);
            await context.SaveChangesAsync();

            return Ok(semestre);
        }

        // PUT: api/Semestres/{codigo}
        [HttpPut("{codigo}")]
        public async Task<IActionResult> UpdateSemestre(
            string codigo,
            [FromBody] Semestre semestre
        )
        {
            var existing = await context.Semestres
                .Where(s => s.Codigo == codigo && s.Estado != "Borrado")
                .FirstOrDefaultAsync();

            if (existing == null)
                return NotFound();

            existing.Anio = semestre.Anio;
            existing.FechaInicio = semestre.FechaInicio;
            existing.FechaFin = semestre.FechaFin;

            await context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Semestres/{codigo}
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> DeleteSemestre(string codigo)
        {
            var semestre = await (from sem in context.Semestres
                                  where sem.Codigo == codigo
                                  && sem.Estado != "Borrado"
                                  select sem).FirstOrDefaultAsync();

            if (semestre == null)
                return NotFound();

            semestre.Estado = "Borrado";
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
