using Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entidades;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InscriptionController : ControllerBase
    {
        private readonly AppDbContext context;
        public InscriptionController(AppDbContext context)
        {
            this.context =context;
        }
        [HttpGet("ListarInscripciones")]
        public async Task<ActionResult<IEnumerable<Inscripcion>>> GetInscripcion()
        {
             return await(from ar in context.Inscripciones
                           where ar.estado == "activo"
                           select ar).ToListAsync();
            
        }
        [HttpGet("Mostrar segun el codigo")]
        public async Task<ActionResult<IEnumerable<Inscripcion>>> GetInscripcion(string codigo)
        {
             return await(from ar in context.Inscripciones
                           where ar.codigoDeInscripcion == codigo
                           select ar).ToListAsync();
            
        }
        [HttpPut("actulizar")]
        public async Task<IActionResult> PutInscripcion(Inscripcion proyecto)
        {
            var db = await context.Inscripciones
                .FirstOrDefaultAsync(x => x.codigoDeInscripcion == proyecto.codigoDeInscripcion && x.estado == "activo");

            if (db == null)
                return NotFound("No existe ese codigo.");
            db.estudianteCi = proyecto.estudianteCi;
            db.codigoCarrera = proyecto.codigoCarrera;
            db.fechaDeInscripcion = proyecto.fechaDeInscripcion;
            await context.SaveChangesAsync();
            return Ok("Se actualizo correctamente");
        }
        [HttpPost]
    public async Task<ActionResult<Inscripcion>> PostInscripcion(Inscripcion archivo )
    {
            var xd = await (from ar in context.Inscripciones
                            where ar.codigoDeInscripcion ==archivo.codigoDeInscripcion
                            select ar).FirstOrDefaultAsync();
            if(xd != null)
            {
                return BadRequest("No se puede realizar esta incripcion porque ya existe");
            }
        await context.Inscripciones.AddAsync(archivo);
        await context.SaveChangesAsync();
        return Ok(archivo);
    }
        [HttpDelete]
        public async Task<ActionResult<Inscripcion>> DeleteInscripcion(string codigo)
        {
            
            var xd = await (from ar in context.Inscripciones
                             where ar.codigoDeInscripcion == codigo
                             select ar).FirstAsync();
            xd.estado="inactivo";
            await context.SaveChangesAsync();
            return Ok("Inscripcion eliminada correctamente");
        }

    }
}