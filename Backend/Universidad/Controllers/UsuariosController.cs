using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Universidad.Entidades;
using Universidad.Core.DTOs;
using Universidad.Core.Mapedores;
using Entidades;

namespace Universidad.Presentacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Usuario_CajasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public Usuario_CajasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Usuario_Cajas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario_CajaDTO>>> GetUsuario_Caja()
        {
            List<Usuario_Caja> l = await _context.Usuarios_Caja.ToListAsync();
            List<Usuario_CajaDTO> lg = new List<Usuario_CajaDTO>();
            foreach ( Usuario_Caja i in l)
            {
                lg.Add(i.toUsuario_CajaDTO());
            }
            return lg;
        }

        // GET: api/Usuario_Cajas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario_CajaDTO>> GetUsuario_Caja(int id)
        {
            var Usuario_Caja = await _context.Usuarios_Caja.FindAsync(id);

            if (Usuario_Caja == null)
            {
                return NotFound();
            }

            return Usuario_Caja.toUsuario_CajaDTO();
        }

        // PUT: api/Usuario_Cajas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario_Caja(int id, Usuario_CajaDTO Usuario_Caja)
        {
            if (id != Usuario_Caja.Id)
            {
                return BadRequest();
            }

            _context.Entry(Usuario_Caja).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Usuario_CajaExists(id))
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

        // POST: api/Usuario_Cajas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuario_CajaDTO>> PostUsuario_Caja(Usuario_Caja Usuario_Caja)
        {
            _context.Usuarios_Caja.Add(Usuario_Caja);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario_Caja", new { id = Usuario_Caja.Id }, Usuario_Caja.toUsuario_CajaDTO());
        }

        // DELETE: api/Usuario_Cajas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario_Caja(int id)
        {
            var Usuario_Caja = await _context.Usuarios_Caja.FindAsync(id);
            if (Usuario_Caja == null)
            {
                return NotFound();
            }

            _context.Usuarios_Caja.Remove(Usuario_Caja);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool Usuario_CajaExists(int id)
        {
            return _context.Usuarios_Caja.Any(e => e.Id == id);
        }
    }
}
