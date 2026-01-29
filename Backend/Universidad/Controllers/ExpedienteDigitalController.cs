using Entidades;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpedienteDigitalController : ControllerBase
    {
        private readonly AppDbContext context;
        public ExpedienteDigitalController(AppDbContext context)
        {
            this.context =context;
        }
        [HttpGet("MOstrarExpedientes")]
        public async Task<ActionResult<IEnumerable<ExpedienteDigital>>> GetExpedienteDigital()
        {
             return await(from ar in context.ExpedientesDigitales
                           where ar.estado != "activo"
                           select ar).ToListAsync();
            
        }
        [HttpGet("FiltrarExpedientePorEstudiante")]
        public async Task<ActionResult<IEnumerable<ExpedienteDigital>>> GetListarArchivos(string codigo)
        {
             return await(from ar in context.ExpedientesDigitales
                           where ar.estudianteCi == codigo
                           select ar).ToListAsync();
        }
        [HttpGet("FiltrarExpedienteFaltantesDelEstudiante")]
        public async Task<ActionResult<IEnumerable<ExpedienteDigital>>> GetListarArchivosFAltantes(string codigo)
        {
             return await(from ar in context.ExpedientesDigitales
                           where ar.estado == "faltante"
                           select ar).ToListAsync();
        }

        [HttpPut("actulizar")]
        public async Task<IActionResult> PutExpedienteDigital(ExpedienteDigital proyecto)
        {
            var db = await context.ExpedientesDigitales
                .FirstOrDefaultAsync(x => x.archivoCode == proyecto.archivoCode);

            if (db == null)
                return NotFound("No existe ese codigo.");
            db.codigoDocumento =proyecto.codigoDocumento;
            db.estudianteCi = proyecto.estudianteCi;
            db.DocenteCi = proyecto.DocenteCi;
            db.nombreArchivo  = proyecto.nombreArchivo;
            db.observaciones = proyecto.observaciones;
            db.estado =proyecto.estado;
        
            await context.SaveChangesAsync();
            return Ok("Se actualizo correctamente");
        }

        [HttpPost]
        public async Task<ActionResult<ExpedienteDigital>> PostExpedienteDigital(ExpedienteDigital archivo )
        {
                var xd = await (from ar in context.ExpedientesDigitales
                                where ar.archivoCode ==archivo.archivoCode
                                select ar).FirstOrDefaultAsync();
                if(xd != null)
                {
                    return BadRequest("El expediente con este codigo ya existe");
                }
            await context.ExpedientesDigitales.AddAsync(archivo);
            await context.SaveChangesAsync();
            return Ok(archivo);
        }
        [HttpDelete]
        public async Task<ActionResult<ExpedienteDigital>> DeleteExpedienteDigital(string codigo)
        {
            
            var xd = await (from ar in context.ExpedientesDigitales
                             where ar.archivoCode == codigo
                             select ar).FirstAsync();
            xd.estado="inactivo";
            await context.SaveChangesAsync();
            return Ok("Expediente eliminado correctamente");
        }

    }
}