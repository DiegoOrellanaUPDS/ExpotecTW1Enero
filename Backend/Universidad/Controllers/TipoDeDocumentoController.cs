using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entidades;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoDocumentoController : ControllerBase
    {
        private readonly AppDbContext context;
        public TipoDocumentoController(AppDbContext context)
        {
            this.context =context;
        }
        [HttpGet("ListarDocumentos")]
        public async Task<ActionResult<IEnumerable<TipoDocumentos>>> GetTipoDocumento()
        {
             return await(from ar in context.TiposDocumentos
                           where ar.estado == "activo"
                           select ar).ToListAsync();
            
        }
        [HttpGet("filtrarArchivosDeEstudiantes")]
        public async Task<ActionResult<IEnumerable<TipoDocumentos>>> GetTipoDocumentoEstudiantes()
        {
             return await(from ar in context.TiposDocumentos
                           where ar.aplicacion == "estudiantes"
                           select ar).ToListAsync();
            
        }
        [HttpGet("filtrarArchivosDeDocentes")]
        public async Task<ActionResult<IEnumerable<TipoDocumentos>>> GetTipoDocumentoDocentes()
        {
             return await(from ar in context.TiposDocumentos
                           where ar.aplicacion == "docentes"
                           select ar).ToListAsync();
            
        }
        [HttpPut("actulizar")]
        public async Task<IActionResult> PutTipoDocumentos(TipoDocumentos proyecto)
        {
            var db = await context.TiposDocumentos
                .FirstOrDefaultAsync(x => x.codigoDocumento == proyecto.codigoDocumento && x.estado == "activo");

            if (db == null)
                return NotFound("No existe ese codigo.");
            db.nombreDocumento = proyecto.nombreDocumento;
            db.aplicacion = proyecto.aplicacion;
            db.obligatorio = proyecto.obligatorio;
            db.descripcion = proyecto.descripcion;
            await context.SaveChangesAsync();
            return Ok($"Se actualizó el codigo: {proyecto.codigoDocumento}");
        }
        [HttpPost]
    public async Task<ActionResult<TipoDocumentos>> PostTipoDocumento(TipoDocumentos archivo )
    {
            var xd = await (from ar in context.TiposDocumentos
                            where ar.codigoDocumento ==archivo.codigoDocumento
                            select ar).FirstOrDefaultAsync();
            if(xd != null)
            {
                return BadRequest("El Documento con este codigo ya existe");
            }
        await context.TiposDocumentos.AddAsync(archivo);
        await context.SaveChangesAsync();
        return Ok(archivo);
    }
        [HttpDelete]
        public async Task<ActionResult<TipoDocumentos>> DeleteTipoDocumento(string codigo)
        {
            
            var xd = await (from ar in context.TiposDocumentos
                             where ar.codigoDocumento == codigo
                             select ar).FirstAsync();
            xd.estado="inactivo";
            await context.SaveChangesAsync();
            return Ok("Documento eliminado correctamente");
        }

    }
}