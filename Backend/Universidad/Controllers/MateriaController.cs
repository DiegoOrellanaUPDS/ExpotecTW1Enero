using Entidades;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FacultadIngenieria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MateriaController : ControllerBase
    {
        private readonly AppDbContext context;

        public MateriaController(AppDbContext context)
        {
            this.context = context;
        }

        // 🔹 GET: api/materia
        [HttpGet]
        public async Task<ActionResult<List<Materia>>> GetMaterias()
        {
            var materias = await context.Materias
                .Where(m => m.Estado)
                .ToListAsync();

            return Ok(materias);
        }

        // 🔹 GET por código
        [HttpGet("{cod}")]
        public async Task<ActionResult<Materia>> GetMateriaCodigo(string cod)
        {
            var materia = await context.Materias
                .Where(m => m.Codigo == cod && m.Estado)
                .FirstOrDefaultAsync();

            if (materia == null)
                return NotFound("Materia no encontrada");

            return Ok(materia);
        }

        // 🔹 POST
        [HttpPost]
        public async Task<ActionResult> CreateMateria([FromBody] Materia materia)
        {
            var existe = await context.Materias
                .AnyAsync(m => m.Codigo == materia.Codigo && m.Estado);

            if (existe)
                return BadRequest("Ya existe una materia con ese código");

            materia.Estado = true;

            await context.Materias.AddAsync(materia);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMateriaCodigo),
                new { cod = materia.Codigo }, materia);
        }

        // 🔹 PUT
        [HttpPut("{codigo}")]
        public async Task<IActionResult> PutMateria(string codigo, [FromBody] Materia materia)
        {
            var existente = await context.Materias
                .Where(m => m.Codigo == codigo && m.Estado)
                .FirstOrDefaultAsync();

            if (existente == null)
                return NotFound("Materia no encontrada");

            // Solo actualizamos nombre (no código)
            existente.Nombre = materia.Nombre;

            await context.SaveChangesAsync();
            return NoContent();
        }

        // 🔹 DELETE lógico
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> DeleteMateria(string codigo)
        {
            var materia = await context.Materias
                .Where(m => m.Codigo == codigo && m.Estado)
                .FirstOrDefaultAsync();

            if (materia == null)
                return NotFound("Materia no encontrada");

            materia.Estado = false;
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
