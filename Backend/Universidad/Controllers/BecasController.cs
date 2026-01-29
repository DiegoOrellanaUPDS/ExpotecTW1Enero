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
    //[Authorize] // Protege todo el controlador
    public class BecasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BecasController(AppDbContext context)
        {
            _context = context;
        }

        // 1. LISTAR TODAS LAS BECAS (GET: api/becas)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Beca>>> GetBecas()
        {
            return await _context.Becas.ToListAsync();
        }

        // 2. OBTENER UNA BECA POR ID (GET: api/becas/5)
        [HttpGet("{id}")]
        public async Task<ActionResult<Beca>> GetBeca(int id)
        {
            var beca = await _context.Becas.FindAsync(id);

            if (beca == null)
            {
                return NotFound();
            }

            return beca;
        }

        // 3. CREAR NUEVA BECA (POST: api/becas)
        [HttpPost]
        public async Task<ActionResult<Beca>> PostBeca(Beca beca)
        {
            _context.Becas.Add(beca);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBeca), new { id = beca.Id }, beca);
        }

        // 4. ACTUALIZAR BECA (PUT: api/becas/5)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBeca(int id, Beca beca)
        {
            if (id != beca.Id)
            {
                return BadRequest();
            }

            _context.Entry(beca).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BecaExists(id))
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

        // 5. ELIMINAR BECA (DELETE: api/becas/5)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBeca(int id)
        {
            var beca = await _context.Becas.FindAsync(id);
            if (beca == null)
            {
                return NotFound();
            }

            _context.Becas.Remove(beca);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BecaExists(int id)
        {
            return _context.Becas.Any(e => e.Id == id);
        }
    }
}
