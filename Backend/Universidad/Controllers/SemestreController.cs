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
    public class SemestresController : ControllerBase
    {
        private readonly AppDbContext context;

        public SemestresController(AppDbContext context)
        {
            this.context = context;
        }

        // 🔹 Método auxiliar para validar sesión
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




        // GET: api/Semestres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Semestre>>> GetSemestres()
        {
            if (!VerificarSesion(out var usuario))
                return Unauthorized("Debes iniciar sesión.");

            return Ok(await context.Semestres
                .Where(s => s.Estado != "Borrado")
                .ToListAsync());
        }

        // GET: api/Semestres/{codigo}
        [HttpGet("{codigo}")]
        public async Task<IActionResult> GetSemestre(string codigo)
        {
            if (!VerificarSesion(out var usuario))
                return Unauthorized("Debes iniciar sesión.");

            var semestre = await context.Semestres
                .Where(s => s.Codigo == codigo && s.Estado != "Borrado")
                .FirstOrDefaultAsync();

            if (semestre == null)
                return NotFound();

            return Ok(semestre);
        }

        // POST: api/Semestres
        [HttpPost]
        public async Task<IActionResult> CreateSemestre(Semestre semestre)
        {
            if (!VerificarSesion(out var usuario))
                return Unauthorized("Debes iniciar sesión.");

            var existe = await context.Semestres
                .Where(s => s.Codigo == semestre.Codigo && s.Estado != "Borrado")
                .FirstOrDefaultAsync();

            if (existe != null)
                return BadRequest("El semestre ya existe.");

            semestre.Estado = "Activo";

            await context.Semestres.AddAsync(semestre);
            await context.SaveChangesAsync();

            return Ok(semestre);
        }

        // PUT: api/Semestres/{codigo}
        [HttpPut("{codigo}")]
        public async Task<IActionResult> UpdateSemestre(string codigo, [FromBody] Semestre semestre)
        {
            if (!VerificarSesion(out var usuario))
                return Unauthorized("Debes iniciar sesión.");

            var existing = await context.Semestres
                .Where(s => s.Codigo == codigo && s.Estado != "Borrado")
                .FirstOrDefaultAsync();

            if (existing == null)
                return NotFound();

            existing.Anio = semestre.Anio;
            existing.FechaInicio = semestre.FechaInicio;
            existing.FechaFin = semestre.FechaFin;

            await context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Semestres/{codigo}
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> DeleteSemestre(string codigo)
        {
            if (!VerificarSesion(out var usuario))
                return Unauthorized("Debes iniciar sesión.");

            var semestre = await context.Semestres
                .Where(s => s.Codigo == codigo && s.Estado != "Borrado")
                .FirstOrDefaultAsync();

            if (semestre == null)
                return NotFound();

            semestre.Estado = "Borrado";
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
