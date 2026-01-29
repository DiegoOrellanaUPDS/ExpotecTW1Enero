using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Universidad.Entidades;

namespace Universidad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LimpiezaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LimpiezaController(AppDbContext context)
        {
            _context = context;
        }

        // --- ENDPOINT 1: REGISTRAR INSUMOS (POST) ---
        [HttpPost("insumos")]
        public async Task<IActionResult> RegistrarInsumo([FromBody] LimpiezaInsumo insumo)
        {
            if (insumo == null) return BadRequest("Datos vacíos");

            _context.LimpiezaInsumos.Add(insumo);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Insumo registrado correctamente", datos = insumo });
        }

        // --- ENDPOINT 2: REGISTRAR OBJETO PERDIDO (POST) ---
        [HttpPost("objetos-perdidos")]
        public async Task<IActionResult> RegistrarObjetoPerdido([FromBody] ObjetoPerdido objeto)
        {
            if (objeto == null) return BadRequest("Datos vacíos");

            // Forzamos la fecha actual si no la mandan
            if (objeto.FechaEncontrado == default) objeto.FechaEncontrado = DateTime.UtcNow;

            _context.ObjetosPerdidos.Add(objeto);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Objeto perdido registrado. ¡Ojalá aparezca el dueño!", datos = objeto });
        }
    }
}