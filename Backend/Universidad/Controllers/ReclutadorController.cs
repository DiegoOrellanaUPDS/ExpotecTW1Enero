using System.Formats.Asn1;
using Data;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReclutadorController:ControllerBase
    {
        private readonly AppDbContext context;
        public ReclutadorController(AppDbContext context)
        {
            this.context=context;
        }

        [HttpGet("listar-reclutadores")]
        public async Task<IActionResult> GetReclutadores()
        {
            return Ok(await (context.Reclutadores).ToListAsync());
        }




        [HttpGet("{id}")]
        public async Task<IActionResult> GetReclutador(int id)
        {
            var reclutador = await context.Reclutadores.FindAsync(id);

            if (reclutador == null)
            {
                return NotFound(new { mensaje = "Reclutador no encontrado" });
            }

            return Ok(reclutador);
        }

 
        [HttpPost]
        public async Task<IActionResult> PostReclutador(Reclutador reclutador)
        {
            // var user=await (from u in context.UsuarioTHs);
            context.Reclutadores.Add(reclutador);
            await context.SaveChangesAsync();

            return Ok("Usuario registrado");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutReclutador(string id, Reclutador reclutador)
        {
            var recl=await (from r in context.Reclutadores
            where r.CodigoReclutador==id select r).FirstOrDefaultAsync();
            if (recl==null)
            {
                return BadRequest("No se encontro el reclutador");
            }
            recl=reclutador;
            await context.SaveChangesAsync();
            return Ok("Se edito correctamente el reclutador");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReclutador(string id)
        {
            var reclutador = await context.Reclutadores.FindAsync(id);
            if (reclutador == null)
            {
                return NotFound();
            }

            context.Reclutadores.Remove(reclutador);
            await context.SaveChangesAsync();

            return Ok("Reclutador eliminado correctamente");
        }




    }
}