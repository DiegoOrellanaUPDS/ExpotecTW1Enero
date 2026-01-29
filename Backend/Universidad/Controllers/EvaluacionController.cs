using Data;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EvaluacionController : ControllerBase
    {
        private readonly AppDbContext context;

        public EvaluacionController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostEvaluacion(Evaluacion evaluacion)
        {
            var existeReclutador = await context.Reclutadores.AnyAsync(r => r.CodigoReclutador == evaluacion.CodigoReclutador);
            var existePostulante = await context.Postulantes.AnyAsync(p => p.CodigoPostulante == evaluacion.CodigoPostulante);

            if (!existeReclutador || !existePostulante)
            {
                return BadRequest("El Reclutador o el Postulante no existen.");
            }

            context.Evaluaciones.Add(evaluacion);
            await context.SaveChangesAsync();
            return Ok(evaluacion);
        }

        [HttpGet("historial/{idPostulante}")]
        public async Task<IActionResult> GetHistorial(string idPostulante)
        {
            return Ok(await (from p in context.Evaluaciones
            where p.CodigoPostulante==idPostulante select p).ToListAsync());
        }
    }
}