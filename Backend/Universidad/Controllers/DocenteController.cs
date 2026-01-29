using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
namespace Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class DocenteController : ControllerBase
    {

        private readonly AppDbContext context;
        public DocenteController(AppDbContext context)
        {
            this.context =context;
        }
        [HttpGet("ListarDocentes")]
        public async Task<ActionResult<IEnumerable<Docente>>> GetDocentes()
        {
             return await(from ar in context.Docentes
                           where ar.estado == "activo"
                           select ar).ToListAsync();
            
        }
        [HttpGet("MostrarSegunElCi")]
        public async Task<ActionResult<IEnumerable<Docente>>> GetDocentes(string codigo)
        {
             return await(from ar in context.Docentes
                           where ar.docenteCi == codigo
                           select ar).ToListAsync();
            
        }

        [HttpPut("actulizar")]
        public async Task<IActionResult> PutDocente(Docente proyecto)
        {
            var db = await context.Docentes
                .FirstOrDefaultAsync(x => x.docenteCi == proyecto.docenteCi);

            if (db == null)
                return NotFound("No existe ese codigo.");
            db.nombreDocente = proyecto.nombreDocente;
            db.apellidoDocente = proyecto.apellidoDocente;
            db.fechaDeNacimiento = proyecto.fechaDeNacimiento;
            db.genero = proyecto.genero;
            db.emailPersonal = proyecto.emailPersonal;
            db.emailInstitucional =proyecto.emailInstitucional;
            db.telefono =proyecto.telefono;
            db.direccion =proyecto.direccion;
            db.gradoAcademido = proyecto.gradoAcademido;
            db.fechaDeIngreso = proyecto.fechaDeIngreso;
            await context.SaveChangesAsync();
            return Ok("Se actualizo correctamente");
        }

        [HttpPost]
    public async Task<ActionResult<Docente>> PostDocente(Docente archivo )
    {
            var xd = await (from ar in context.Docentes
                            where ar.docenteCi ==archivo.docenteCi
                            select ar).FirstOrDefaultAsync();
            if(xd != null)
            {
                return BadRequest("El docente con este ci ya existe");
            }
        await context.Docentes.AddAsync(archivo);
        await context.SaveChangesAsync();
        return Ok(archivo);
    }
        [HttpDelete]
        public async Task<ActionResult<Docente>> DeleteDocente(string codigo)
        {
            
            var xd = await (from ar in context.Docentes
                             where ar.docenteCi == codigo
                             select ar).FirstAsync();
            xd.estado="Inactivo";
            await context.SaveChangesAsync();
            return Ok("Docente eliminado correctamente");
        }
    }
}

