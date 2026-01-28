using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Universidad.Data; // Ajusta el namespace según tu proyecto
using Universidad.Models;

namespace Universidad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // <--- REQUISITO OBLIGATORIO: Protege todo el controlador
    public class DepartamentosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DepartamentosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. LISTAR (GET: api/departamentos)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Departamento>>> GetDepartamentos()
        {
            return await _context.Departamentos.ToListAsync();
        }

        // 2. OBTENER UNO (GET: api/departamentos/5)
        [HttpGet("{id}")]
        public async Task<ActionResult<Departamento>> GetDepartamento(int id)
        {
            var departamento = await _context.Departamentos.FindAsync(id);

            if (departamento == null)
            {
                return NotFound();
            }

            return departamento;
        }

        // 3. REGISTRAR (POST: api/departamentos)
        [HttpPost]
        public async Task<ActionResult<Departamento>> PostDepartamento(Departamento departamento)
        {
            _context.Departamentos.Add(departamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDepartamento", new { id = departamento.Id }, departamento);
        }

        // 4. MODIFICAR (PUT: api/departamentos/5)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartamento(int id, Departamento departamento)
        {
            if (id != departamento.Id)
            {
                return BadRequest("El ID de la URL no coincide con el del cuerpo");
            }

            _context.Entry(departamento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartamentoExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // 5. DAR DE BAJA LÓGICA (PATCH: api/departamentos/5/estado)
        // Es mejor cambiar el estado que borrar el registro físico
        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] bool nuevoEstado)
        {
            var departamento = await _context.Departamentos.FindAsync(id);
            if (departamento == null) return NotFound();

            departamento.Estado = nuevoEstado;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DepartamentoExists(int id)
        {
            return _context.Departamentos.Any(e => e.Id == id);
        }
    }
}