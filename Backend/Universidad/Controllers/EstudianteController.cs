using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entidades;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstudianteController : ControllerBase
    {
        private readonly AppDbContext context;
        public EstudianteController(AppDbContext context)
        {
            this.context =context;
        }
         [HttpGet("EstudiantesDesHabilitados")]
        public async Task<ActionResult<IEnumerable<Estudiante>>> GetEstudianteDeshabilitado()
        {
             return await(from ar in context.Estudiantes
                           where ar.estado != "activo"
                           select ar).ToListAsync();
            
        }
        [HttpGet("EstudiantesHabilitados")]
        public async Task<ActionResult<IEnumerable<Estudiante>>> GetEstudianteHabilitado()
        {
             return await(from ar in context.Estudiantes
                           where ar.estado == "activo"
                           select ar).ToListAsync();
            
        }
        [HttpGet("MostrarSegunElCi")]
        public async Task<ActionResult<IEnumerable<Estudiante>>> GetEstudiante(string codigo)
        {
             return await(from ar in context.Estudiantes
                           where ar.estudianteCi == codigo
                           select ar).ToListAsync();
            
        }
        [HttpPut("actualizar")]
        public async Task<IActionResult> PutEstudiante(Estudiante proyecto)
        {
            var db = await context.Estudiantes
                .FirstOrDefaultAsync(x => x.estudianteCi == proyecto.estudianteCi && x.estado == "activo");

            if (db == null)
                return NotFound("No existe ese codigo.");
            db.nombreEstudiante = proyecto.nombreEstudiante;
            db.apellidoEstudiante = proyecto.apellidoEstudiante;
            db.FechaDeNacimiento = proyecto.FechaDeNacimiento;
            db.genero = proyecto.genero;
            db.emailPersonal = proyecto.emailPersonal;
            db.emailInstitucional =proyecto.emailInstitucional;
            db.telefono =proyecto.telefono;
            db.direccion =proyecto.direccion;
            db.fechaDeIngreso = proyecto.fechaDeIngreso;
            db.pocentajeDeBeca =proyecto.pocentajeDeBeca;
            db.tipoDeBeca = proyecto.tipoDeBeca;
         
            await context.SaveChangesAsync();
            return Ok("Estudiante actualizado correctamente");
        }
        [HttpPost]
    public async Task<ActionResult<Estudiante>> PostEstudiante(Estudiante archivo )
    {
            var xd = await (from ar in context.Estudiantes
                            where ar.estudianteCi ==archivo.estudianteCi
                            select ar).FirstOrDefaultAsync();
            if(xd != null)
            {
                return BadRequest("El estudiante con este ci ya existe");
            }
        await context.Estudiantes.AddAsync(archivo);
        await context.SaveChangesAsync();
        return Ok(archivo);
    }
        [HttpDelete]
        public async Task<ActionResult<Estudiante>> DeleteEstudiante(string codigo)
        {
            
            var xd = await (from ar in context.Estudiantes
                             where ar.estudianteCi == codigo
                             select ar).FirstAsync();
            xd.estado="inactivo";
            await context.SaveChangesAsync();
            return Ok("El Estudiante fue eliminado correctamente");
        }

    }
}