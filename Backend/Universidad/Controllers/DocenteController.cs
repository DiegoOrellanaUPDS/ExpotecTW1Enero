using Entidades;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FacultadIngenieria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocenteController : ControllerBase
    {
        private AppDbContext context;
            
        public DocenteController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetDocentes()
        {
            return Ok(await context.Docentes.Where(doc => doc.Estado != false).ToListAsync());
        }

        [HttpGet("{cod}")]
        public async Task<IActionResult> GetDocenteCodigo(string cod)
        {
            var docente = await (from doc in context.Docentes
                                   where doc.Codigo == cod && doc.Estado != false
                                   select doc).FirstOrDefaultAsync();
            if (docente == null)
                return NotFound();
            return Ok(docente);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDocente(Docente docente)
        {
            var e = await (from doc in context.Docentes
                           where doc.Codigo == docente.Codigo && doc.Estado != false
                           select doc).FirstOrDefaultAsync();
            if (e != null)
                return BadRequest("El docente ya se encuentra en la base de datos");
        
            await context.Docentes.AddAsync(docente);
            await context.SaveChangesAsync();
            return 
            Ok(docente);
        }
        [HttpPut("{codigo}")]
        public async Task<IActionResult> PutDocente(string codigo, [FromBody] Docente docente)
        {
            var pe = await (from doc in context.Docentes
                                   where doc.Codigo == codigo && doc.Estado != false
                                   select doc).FirstOrDefaultAsync();
            if (pe == null)
                return NotFound();
        
            pe.Email = docente.Email;
            pe.Telefono = docente.Telefono;
            pe.Codigo = docente.Codigo;
            pe.FechaContratacion = docente.FechaContratacion;
            await context.SaveChangesAsync();
            return NoContent();
        }
        

        [HttpDelete("{codigo}")]
        public async Task<IActionResult> DeleteDocente(string codigo)
        {
            var docente = await (from doc in context.Docentes
                                   where doc.Codigo == codigo && doc.Estado != false
                                   select doc).FirstOrDefaultAsync();
            if (docente == null)
                return NotFound();
        
            docente.Estado = false;
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
