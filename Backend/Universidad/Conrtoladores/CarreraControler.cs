using Entidades;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Universidad.Data;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CareerController : ControllerBase
    {
        private readonly AppDbContext context;
        public CareerController(AppDbContext context)
        {
            this.context =context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Carrera>>> GetCareer()
        {
             return await(from ar in context.Carreras
                           where ar.estado == "activo"
                           select ar).ToListAsync();
            
        }
        [HttpGet("Mostrar segun el codigo")]
        public async Task<ActionResult<IEnumerable<Carrera>>> GetCareers(string codigo)
        {
             return await(from ar in context.Carreras
                           where ar.codigoCarrera == codigo
                           select ar).ToListAsync();
            
        }
        [HttpPost]
    public async Task<ActionResult<Carrera>> PostCareer(Carrera archivo )
    {
            var xd = await (from ar in context.Carreras
                            where ar.codigoCarrera ==archivo.codigoCarrera
                            select ar).FirstOrDefaultAsync();
            if(xd != null)
            {
                return BadRequest("El archivo con este codigo ya existe");
            }
        await context.Carreras.AddAsync(archivo);
        await context.SaveChangesAsync();
        return Ok(archivo);
    }
        [HttpDelete]
        public async Task<ActionResult<Carrera>> DeleteCareer(string codigo)
        {
            
            var xd = await (from ar in context.Carreras
                             where ar.codigoCarrera == codigo
                             select ar).FirstAsync();
            xd.estado="Inactivo";
            await context.SaveChangesAsync();
            return Ok("Archivo eliminado correctamente");
        }

    }
}