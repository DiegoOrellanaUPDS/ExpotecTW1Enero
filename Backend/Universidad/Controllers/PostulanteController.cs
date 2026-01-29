using Data;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostulanteController : ControllerBase
    {
        private readonly AppDbContext context;

        public PostulanteController(AppDbContext context)
        {
            this.context = context;
        }

        // Obtener todos los postulantes
        [HttpGet]
        public async Task<IActionResult> GetPostulantes()
        {
            return Ok(await (from p in context.Postulantes where p.Estado==true select p).ToListAsync());
        }

        [HttpGet("postulante-por-trabajo/{idTrabajo}")]
        public async Task<IActionResult> GetPostulantesPorTrabajo(string idTrabajo)
        {
            return Ok(await (from p in context.Postulantes where p.CodigoTrabajo==idTrabajo select p).ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> PostPostulante(Postulante postulante)
        {
            context.Postulantes.Add(postulante);
            await context.SaveChangesAsync();
            return Ok(postulante);
        }
        [HttpDelete]
        public async Task<IActionResult> DeletePostulante(string idPostulante)
        {
            var pos=await (from p in context.Postulantes where p.CodigoPostulante==idPostulante select p).FirstOrDefaultAsync();
            if (pos==null)
            {
                return NotFound("No se pudo encontrar el postulante");
            }
            pos.Estado=false;
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}