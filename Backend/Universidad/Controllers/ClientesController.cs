using Data;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Caja.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClienteController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/cliente
        [HttpGet]
        public async Task<ActionResult<List<Cliente>>> GetClientes()
        {
            var clientes = await _context.Clientes
                .AsNoTracking()
                .Where(c => c.Estado == true)
                .ToListAsync();

            return Ok(clientes);
        }

        // GET: api/cliente/{codigo}
        [HttpGet("{codigo}")]
        public async Task<ActionResult<Cliente>> GetCliente(string codigo)
        {
            var cliente = await _context.Clientes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.codigo == codigo && c.Estado == true);

            if (cliente == null)
                return NotFound();

            return Ok(cliente);
        }

        // GET: api/cliente/borrados
        [HttpGet("borrados")]
        public async Task<ActionResult<List<Cliente>>> GetClientesBorrados()
        {
            var clientes = await _context.Clientes
                .AsNoTracking()
                .Where(c => c.Estado == false)
                .ToListAsync();

            return Ok(clientes);
        }

        // POST: api/cliente
        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(string codigo, string? ci)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                return BadRequest("El código es obligatorio");

            var existe = await _context.Clientes.AnyAsync(c => c.codigo == codigo);
            if (existe)
                return BadRequest("Ya existe un cliente con ese código");
            /*var existePersona = await _context.Personas.AnyAsync(c => c.ci == ci);
            if (existePersona)
                return BadRequest("No existe una persona con ese ci");*/

            var cliente = new Cliente
            {
                codigo = codigo,
                ci = ci,
                Estado = true
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { codigo = cliente.codigo }, cliente);
        }

        // PUT: api/cliente/{codigo}
        [HttpPut("{codigo}")]
        public async Task<ActionResult<Cliente>> PutCliente(string codigo, string codigoNuevo, string? ci)
        {
            if (string.IsNullOrWhiteSpace(codigo) || string.IsNullOrWhiteSpace(codigoNuevo))
                return BadRequest("Código inválido");

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.codigo == codigo && c.Estado == true);

            if (cliente == null)
                return NotFound();

            cliente.codigo = codigoNuevo;
            cliente.ci = ci;

            await _context.SaveChangesAsync();

            return Ok(cliente);
        }

        // DELETE: api/cliente/{codigo}
        [HttpDelete("{codigo}")]
        public async Task<ActionResult<Cliente>> DeleteCliente(string codigo)
        {
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.codigo == codigo && c.Estado == true);

            if (cliente == null)
                return NotFound();

            cliente.Estado = false;
            await _context.SaveChangesAsync();

            return Ok(cliente);
        }

        // PUT: api/cliente/habilitar/{codigo}
        [HttpPut("habilitar/{codigo}")]
        public async Task<ActionResult<Cliente>> HabilitarCliente(string codigo)
        {
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.codigo == codigo && c.Estado == false);

            if (cliente == null)
                return NotFound();

            cliente.Estado = true;
            await _context.SaveChangesAsync();

            return Ok(cliente);
        }
    }
}
