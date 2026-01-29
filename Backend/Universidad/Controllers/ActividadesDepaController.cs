using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using Entidades; 

using Data; // Asegúrate que este sea el namespace de tu DbContext


=======
using Data;
using Entidades;
>>>>>>> feature/rectorado-departamento

namespace Universidad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
<<<<<<< HEAD

    //[Authorize] // Requisito obligatorio del proyecto

=======
>>>>>>> feature/rectorado-departamento
    public class ActividadesDepaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ActividadesDepaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActividadDepa>>> GetActividades()
        {
            return await _context.ActividadesDepa.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActividadDepa>> GetActividad(int id)
        {
            var actividad = await _context.ActividadesDepa.FindAsync(id);
            if (actividad == null) return NotFound();
            return actividad;
        }

        [HttpPost]
        public async Task<ActionResult<ActividadDepa>> PostActividad(ActividadDepa actividad)
        {
            // Validar que el departamento exista
            var existeDepa = await _context.Departamentos.AnyAsync(d => d.Id == actividad.DepartamentoId);
            if (!existeDepa) return BadRequest($"El Departamento {actividad.DepartamentoId} no existe.");

            _context.ActividadesDepa.Add(actividad);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetActividad", new { id = actividad.Id }, actividad);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutActividad(int id, ActividadDepa actividad)
        {
            if (id != actividad.Id) return BadRequest();
            _context.Entry(actividad).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActividad(int id)
        {
            var actividad = await _context.ActividadesDepa.FindAsync(id);
            if (actividad == null) return NotFound();
            _context.ActividadesDepa.Remove(actividad);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}