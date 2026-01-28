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
    public class ContabilidadPeticionDepartamentoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContabilidadPeticionDepartamentoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ContabilidadPeticionDepartamento
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContabilidadPeticionDepartamento>>> GetAll()
        {
            return await _context.ContabilidadPeticionDepartamentos.ToListAsync();
        }

        // GET: api/ContabilidadPeticionDepartamento/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ContabilidadPeticionDepartamento>> GetById(int id)
        {
            var peticion = await _context.ContabilidadPeticionDepartamentos.FindAsync(id);
            if (peticion == null) return NotFound();
            return peticion;
        }

        // POST: api/ContabilidadPeticionDepartamento
        [HttpPost]
        public async Task<ActionResult<ContabilidadPeticionDepartamento>> Create([FromBody] ContabilidadPeticionDepartamento nuevaPeticion)
        {
            nuevaPeticion.FechaContabilidad = DateTime.UtcNow;
            _context.ContabilidadPeticionDepartamentos.Add(nuevaPeticion);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetById), 
                new { id = nuevaPeticion.IdDepartamentoContabilidad }, 
                nuevaPeticion);
        }

        // PUT: api/ContabilidadPeticionDepartamento/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ContabilidadPeticionDepartamento peticionActualizada)
        {
            if (id != peticionActualizada.IdDepartamentoContabilidad) return BadRequest();
            
            _context.Entry(peticionActualizada).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PeticionExists(id)) return NotFound();
                else throw;
            }
            
            return NoContent();
        }

        // DELETE: api/ContabilidadPeticionDepartamento/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var peticion = await _context.ContabilidadPeticionDepartamentos.FindAsync(id);
            if (peticion == null) return NotFound();

            _context.ContabilidadPeticionDepartamentos.Remove(peticion);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool PeticionExists(int id)
        {
            return _context.ContabilidadPeticionDepartamentos.Any(e => e.IdDepartamentoContabilidad == id);
        }
    }
}