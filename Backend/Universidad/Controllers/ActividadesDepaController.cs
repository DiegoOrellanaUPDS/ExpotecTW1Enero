using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Entidades; 
using Universidad.Data; // Asegúrate que este sea el namespace de tu DbContext

namespace Universidad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] // Requisito obligatorio del proyecto
    public class ActividadesDepaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ActividadesDepaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ActividadesDepa
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActividadDepa>>> GetActividades()
        {
            return await _context.ActividadesDepa.ToListAsync();
        }

        // GET: api/ActividadesDepa/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActividadDepa>> GetActividad(int id)
        {
            var actividad = await _context.ActividadesDepa.FindAsync(id);

            if (actividad == null)
            {
                return NotFound();
            }

            return actividad;
        }

        // POST: api/ActividadesDepa
        [HttpPost]
        public async Task<ActionResult<ActividadDepa>> PostActividad(ActividadDepa actividad)
        {
            // Opcional: Validar que el Departamento exista antes de guardar
            // if (!_context.Departamentos.Any(d => d.Id == actividad.DepartamentoId))
            //    return BadRequest("El Departamento especificado no existe.");

            _context.ActividadesDepa.Add(actividad);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActividad", new { id = actividad.Id }, actividad);
        }

        // PUT: api/ActividadesDepa/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActividad(int id, ActividadDepa actividad)
        {
            if (id != actividad.Id)
            {
                return BadRequest("El ID no coincide");
            }

            _context.Entry(actividad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActividadExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // DELETE: api/ActividadesDepa/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActividad(int id)
        {
            var actividad = await _context.ActividadesDepa.FindAsync(id);
            if (actividad == null)
            {
                return NotFound();
            }

            _context.ActividadesDepa.Remove(actividad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActividadExists(int id)
        {
            return _context.ActividadesDepa.Any(e => e.Id == id);
        }
    }
}