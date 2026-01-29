using Data;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class TrabajoController : ControllerBase

    {
        private readonly AppDbContext context;

        public TrabajoController(AppDbContext context)
        {
            this.context = context;
        }

        // GET: api/Trabajo
        [HttpGet]
        public async Task<IActionResult> GetTrabajos()
        {
            return Ok(await (from t in context.Trabajos where t.Estado==true select t).ToListAsync());
        }

        // GET: api/Trabajo/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrabajo(string id)
        {
            var trabajo = await (from t in context.Trabajos where t.CodigoTrabajo==id select t).FirstOrDefaultAsync();
            if (trabajo == null) return NotFound();
            return Ok(trabajo);
        }

        // POST: api/Trabajo
        [HttpPost]
        public async Task<IActionResult> PostTrabajo(Trabajo trabajo)
        {
            context.Trabajos.Add(trabajo);
            await context.SaveChangesAsync();
            return Ok("Se creo correctamente el trabajo");
        }

        // PUT: api/Trabajo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrabajo(string id, Trabajo trabajo)
        {
            var traba=await (from t in context.Trabajos where t.CodigoRequisito==id select t).FirstOrDefaultAsync();
            if (traba== null) return BadRequest();
            traba=trabajo;

            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrabajo(string id)
        {
            var tr= await (from t in context.Trabajos
            where t.CodigoTrabajo==id select t).FirstOrDefaultAsync();
            if (tr==null)
            {
                return NotFound("No puedes eliminar trabajo no existente");
            }
            tr.Estado=false;
            await context.SaveChangesAsync();
            return Ok("Se elimino el trabajo correctamente");
        }

    }
}