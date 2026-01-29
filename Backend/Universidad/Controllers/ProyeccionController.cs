using Data;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProyeccionesController : ControllerBase
    {
        private readonly AppDbContext context;

        public ProyeccionesController(AppDbContext context)
        {
            this.context = context;
        }

        // 🔐 Método auxiliar de sesión (IGUAL AL DE SEMESTRES / MODULOS)
        private bool VerificarSesion(out UsuarioFCES usuario)
        {
            usuario = null;

            if (!Request.Cookies.TryGetValue("token_sesion", out var token))
            {
                Console.WriteLine("No se recibió cookie de sesión.");
                return false;
            }

            usuario = context.UsuariosFCES
                .FirstOrDefault(u => u.TokenSesion == token && u.Estado == "Activo");

            return usuario != null;
        }

        // GET: api/Proyecciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proyeccion>>> GetProyecciones()
        {
            if (!VerificarSesion(out var usuario))
                return Unauthorized("Debes iniciar sesión.");

            return Ok(await context.Proyecciones
                .Where(p => p.Estado != "Borrado")
                .ToListAsync());
        }

        // GET: api/Proyecciones/{codigo}
        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetProyeccion(string codigo)
        {
            if (!VerificarSesion(out var usuario))
                return Unauthorized("Debes iniciar sesión.");

            var proyeccion = await context.Proyecciones
                .Where(p => p.Codigo == codigo && p.Estado != "Borrado")
                .FirstOrDefaultAsync();

            if (proyeccion == null)
                return NotFound();

            return Ok(proyeccion);
        }

        // POST: api/Proyecciones
        [HttpPost]
        public async Task<IActionResult> CreateProyeccion(Proyeccion proyeccion)
        {
            if (!VerificarSesion(out var usuario))
                return Unauthorized("Debes iniciar sesión.");

            var existe = await context.Proyecciones
                .Where(p => p.Codigo == proyeccion.Codigo && p.Estado != "Borrado")
                .FirstOrDefaultAsync();

            if (existe != null)
                return BadRequest("La proyección ya existe.");

            proyeccion.Estado = "Activo";

            await context.Proyecciones.AddAsync(proyeccion);
            await context.SaveChangesAsync();

            return Ok(proyeccion);
        }

        // PUT: api/Proyecciones/{codigo}
        [HttpPut("{codigo}")]
        public async Task<IActionResult> UpdateProyeccion(string codigo, [FromBody] Proyeccion proyeccion)
        {
            if (!VerificarSesion(out var usuario))
                return Unauthorized("Debes iniciar sesión.");

            var existing = await context.Proyecciones
                .Where(p => p.Codigo == codigo && p.Estado != "Borrado")
                .FirstOrDefaultAsync();

            if (existing == null)
                return NotFound();

            existing.LlaveForaneaIdMateria = proyeccion.LlaveForaneaIdMateria;
            existing.LlaveForaneaIdModulo = proyeccion.LlaveForaneaIdModulo;
            existing.Prerrequisito = proyeccion.Prerrequisito;
            existing.EstadoProyeccion = proyeccion.EstadoProyeccion;

            await context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Proyecciones/{codigo}
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> DeleteProyeccion(string codigo)
        {
            if (!VerificarSesion(out var usuario))
                return Unauthorized("Debes iniciar sesión.");

            var proyeccion = await context.Proyecciones
                .Where(p => p.Codigo == codigo && p.Estado != "Borrado")
                .FirstOrDefaultAsync();

            if (proyeccion == null)
                return NotFound();

            proyeccion.Estado = "Borrado";
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
