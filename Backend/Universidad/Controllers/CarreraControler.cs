using Entidades;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarreraController : ControllerBase
    {
        private readonly AppDbContext context;
        public CarreraController(AppDbContext context)
        {
            this.context =context;
        }
        [HttpGet("listarCarreras")]
        public async Task<ActionResult<IEnumerable<Carrera>>> GetCarrera()
        {
             return await(from ar in context.Carreras
                           where ar.estado == "activo"
                           select ar).ToListAsync();
            
        }
        [HttpGet("MostrarLaCarreraPorCodigo")]
        public async Task<ActionResult<IEnumerable<Carrera>>> GetCarrera(string codigo)
        {
             return await(from ar in context.Carreras
                           where ar.codigoCarrera == codigo
                           select ar).ToListAsync();
            
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> PutCarrera(Carrera proyecto)
        {
            var db = await context.Carreras
                .FirstOrDefaultAsync(x => x.codigoCarrera == proyecto.codigoCarrera && x.estado == "activo");

            if (db == null)
                return NotFound("No existe ese codigo.");
            db.nombreCarrera = proyecto.nombreCarrera;
            db.facultad = proyecto.facultad;
         
            await context.SaveChangesAsync();
            return Ok($"Se actualizó el codigo: {proyecto.codigoCarrera}");
        }

        [HttpPost]
    public async Task<ActionResult<Carrera>> PostCarrera(Carrera archivo )
    {
            var xd = await (from ar in context.Carreras
                            where ar.codigoCarrera ==archivo.codigoCarrera
                            select ar).FirstOrDefaultAsync();
            if(xd != null)
            {
                return BadRequest("La carrera con este codigo ya existe");
            }
        await context.Carreras.AddAsync(archivo);
        await context.SaveChangesAsync();
        return Ok(archivo);
    }
        [HttpDelete]
        public async Task<ActionResult<Carrera>> DeleteCarrera(string codigo)
        {
            
            var xd = await (from ar in context.Carreras
                             where ar.codigoCarrera == codigo
                             select ar).FirstAsync();
            xd.estado="inactivo";
            await context.SaveChangesAsync();
            return Ok("Carrera eliminada correctamente");
        }

    }
}