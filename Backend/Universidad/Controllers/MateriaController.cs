using Entidades;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FacultadIngenieria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MateriaController : ControllerBase
    {
        private AppDbContext context;
            
        public MateriaController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetMaterias()
        {
            return Ok(await context.Materias.Where(mater => mater.Estado != false).ToListAsync());
        }

        [HttpGet("{cod}")]
        public async Task<IActionResult> GetMateriaCodigo(string cod)
        {
            var materia = await (from mater in context.Docentes
                                   where mater.Codigo == cod && mater.Estado != false
                                   select mater).FirstOrDefaultAsync();
            if (materia == null)
                return NotFound();
            return Ok(materia);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMateria(Materia materia)
        {
            var e = await (from mater in context.Docentes
                           where mater.Codigo == materia.Codigo && mater.Estado != false
                           select mater).FirstOrDefaultAsync();
            if (e != null)
                return BadRequest("La materia ya se encuentra en la base de datos");
        
            await context.Materias.AddAsync(materia);
            await context.SaveChangesAsync();
            return 
            Ok(materia);
        }
        [HttpPut("{codigo}")]
        public async Task<IActionResult> PutMateria(string codigo, [FromBody] Materia materia)
        {
            var pe = await (from mater in context.Materias
                                   where mater.Codigo == codigo && mater.Estado != false
                                   select mater).FirstOrDefaultAsync();
            if (pe == null)
                return NotFound();
        
            pe.Nombre = materia.Nombre;
            pe.Codigo = materia.Codigo;

            await context.SaveChangesAsync();
            return NoContent();
        }
        

        [HttpDelete("{codigo}")]
        public async Task<IActionResult> DeleteMateria(string codigo)
        {
            var materia = await (from mater in context.Materias
                                   where mater.Codigo == codigo && mater.Estado != false
                                   select mater).FirstOrDefaultAsync();
            if (materia == null)
                return NotFound();
        
            materia.Estado = false;
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
