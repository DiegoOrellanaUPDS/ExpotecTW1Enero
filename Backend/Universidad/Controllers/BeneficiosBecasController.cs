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
    public class BeneficiosBecasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BeneficiosBecasController(AppDbContext context)
        {
            _context = context;
        }

        // 1. LISTAR TODOS LOS BENEFICIOS (GET: api/beneficiosbecas)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BeneficioBeca>>> GetBeneficios()
        {
            return await _context.BeneficiosBecas.ToListAsync();
        }

        // 2. OBTENER BENEFICIOS POR BECA (GET: api/beneficiosbecas/beca/5)
        [HttpGet("beca/{becaId}")]
        public async Task<ActionResult<IEnumerable<BeneficioBeca>>> GetBeneficiosPorBeca(int becaId)
        {
            var beneficios = await _context.BeneficiosBecas
                .Where(b => b.BecaId == becaId)
                .ToListAsync();

            return beneficios;
        }

        // 3. OBTENER UN BENEFICIO POR ID (GET: api/beneficiosbecas/5)
        [HttpGet("{id}")]
        public async Task<ActionResult<BeneficioBeca>> GetBeneficio(int id)
        {
            var beneficio = await _context.BeneficiosBecas.FindAsync(id);

            if (beneficio == null)
            {
                return NotFound();
            }

            return beneficio;
        }

        // 4. CREAR NUEVO BENEFICIO (POST: api/beneficiosbecas)
        [HttpPost]
        public async Task<ActionResult<BeneficioBeca>> PostBeneficio(BeneficioBeca beneficio)
        {
            _context.BeneficiosBecas.Add(beneficio);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBeneficio), new { id = beneficio.Id }, beneficio);
        }

        // 5. ACTUALIZAR BENEFICIO (PUT: api/beneficiosbecas/5)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBeneficio(int id, BeneficioBeca beneficio)
        {
            if (id != beneficio.Id)
            {
                return BadRequest();
            }

            _context.Entry(beneficio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeneficioExists(id))
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

        // 6. ELIMINAR BENEFICIO (DELETE: api/beneficiosbecas/5)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBeneficio(int id)
        {
            var beneficio = await _context.BeneficiosBecas.FindAsync(id);
            if (beneficio == null)
            {
                return NotFound();
            }

            _context.BeneficiosBecas.Remove(beneficio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BeneficioExists(int id)
        {
            return _context.BeneficiosBecas.Any(e => e.Id == id);
        }
    }
}
