using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Data; // Ajusta el namespace según tu proyecto

using Entidades;

namespace Universidad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class DepartamentosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DepartamentosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Departamento>>> GetDepartamentos()
        {
            return await _context.Departamentos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Departamento>> GetDepartamento(int id)
        {
            var departamento = await _context.Departamentos.FindAsync(id);
            if (departamento == null) return NotFound();
            return departamento;
        }

        [HttpPost]
        public async Task<ActionResult<Departamento>> PostDepartamento(Departamento departamento)
        {
            _context.Departamentos.Add(departamento);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetDepartamento", new { id = departamento.Id }, departamento);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartamento(int id, Departamento departamento)
        {
            if (id != departamento.Id) return BadRequest();
            _context.Entry(departamento).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] bool nuevoEstado)
        {
            var departamento = await _context.Departamentos.FindAsync(id);
            if (departamento == null) return NotFound();
            departamento.Estado = nuevoEstado;
            await _context.SaveChangesAsync();
            return Ok(new { estado = nuevoEstado });
        }
    }
}