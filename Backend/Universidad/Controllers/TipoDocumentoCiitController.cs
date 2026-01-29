using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Universidad.Entidades;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoDocumentoCiitController : ControllerBase
    {
        private readonly AppDbContext context;
        public TipoDocumentoCiitController(AppDbContext context)    
        {
            this.context=context;
        }
        [HttpGet("listar")]
        public async Task<IActionResult> GetTipoDocumentoCiits()
        {
            return Ok(await (from e in context.TipoDocumentoCiits
                             where e.estado == "Activo"
                             select e).ToListAsync());
        }
        [HttpGet("buscar")]
        public async Task<IActionResult> GetTipoDocumentosCiit(string codigo)
        {
            var e = await context.TipoDocumentoCiits
                .FirstOrDefaultAsync(x => x.codigo == codigo && x.estado == "Activo");
        
            if (e == null)
                return NotFound("No se encontró el codigo.");
        
            return Ok(e);
        }
        [HttpPost("crear")]
        public async Task<IActionResult> PostTipoDocumentoCiits(TipoDocumentoCiit e)
        {
            var ver = await context.TipoDocumentoCiits
                .FirstOrDefaultAsync(x => x.codigo == e.codigo);
        
            if (ver != null)
                return NotFound("Ese TipoDocumentoCitts con ese codigo ya existe.");
        
            e.estado = "Activo";
            await context.TipoDocumentoCiits.AddAsync(e);
            await context.SaveChangesAsync();
        
            return Ok(e);
        }
        [HttpPut("actulizar")]
        public async Task<IActionResult> PutTipoDocumentoCiits(TipoDocumentoCiit e)
        {
            var db = await context.TipoDocumentoCiits
                .FirstOrDefaultAsync(x => x.codigo == e.codigo && x.estado == "Activo");
        
            if (db == null)
                return NotFound("No existe ese codigo.");
        
            db.descripcion=e.descripcion;
            db.nombre=e.nombre;
        
            await context.SaveChangesAsync();
            return Ok($"Se actualizó el codigo: {e.codigo}");
        }
        [HttpDelete("eliminar")]
        public async Task<IActionResult> DeleteTipoDocumentoCiit(string codigo)
        {
            var e = await context.TipoDocumentoCiits
                .FirstOrDefaultAsync(x => x.codigo == codigo && x.estado == "Activo");
        
            if (e == null)
                return NotFound("No existe ese codigo para eliminar.");
        
            e.estado = "Borrado";
            await context.SaveChangesAsync();
        
            return Ok($"Se eliminó el TipoDocumentoCitt con codigo: {codigo}");
        }
    }
}