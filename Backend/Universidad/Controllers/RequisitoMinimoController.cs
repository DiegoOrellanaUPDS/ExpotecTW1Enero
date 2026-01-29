using Data;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequisitoMinimoController : ControllerBase
    {
        private readonly AppDbContext context;

        public RequisitoMinimoController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetRequisitos()
        {
            return Ok(await (from rm in context.RequisitoMinimos where rm.Estado == true select rm).ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> PostRequisito(RequisitoMinimo requisito)
        {
            context.RequisitoMinimos.Add(requisito);
            await context.SaveChangesAsync();
            return Ok(requisito);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteRequisitoMinimo(string idRequisitoMinimo)
        {
            var requisitom=await (from rm in context.RequisitoMinimos where rm.CodigoRequisito==idRequisitoMinimo select rm ).FirstOrDefaultAsync();
            if (requisitom==null)
            {
                return BadRequest("No puedes eliminar un usuario que no se ha encontrado");
            }
            requisitom.Estado=false;
            await context.SaveChangesAsync();
            return Ok("Se elimino exitosamente el requisito");
        }
    }
}