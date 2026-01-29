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
    public class ProyectosController : ControllerBase
    {
        private readonly AppDbContext context;
        public ProyectosController(AppDbContext context)
        {
            this.context = context;
        }
        [HttpGet("listar")]
        public async Task<IActionResult> GetProyectos()
        {
            return Ok(await (from p in context.Proyectos
                             where p.estado == "Activo"
                             select p).ToListAsync());
        }
        [HttpGet("buscar")]
        public async Task<IActionResult> GetProyecto(string codigo)
        {
            var p = await context.Proyectos
                .FirstOrDefaultAsync(x => x.codigo == codigo && x.estado == "Activo");

            if (p == null)
                return NotFound("No se encontró el codigo.");

            return Ok(p);
        }
        [HttpPost("crear")]
        public async Task<IActionResult> PostProyecto(Proyecto proyecto)
        {
            var ver = await context.Proyectos
                .FirstOrDefaultAsync(x => x.codigo == proyecto.codigo);

            if (ver != null)
                return NotFound("Ese Proyecto con ese codigo ya existe.");

            proyecto.estado = "Activo";
            await context.Proyectos.AddAsync(proyecto);
            await context.SaveChangesAsync();

            return Ok(proyecto);
        }
        [HttpPut("actulizar")]
        public async Task<IActionResult> PutProyecto(Proyecto proyecto)
        {
            var db = await context.Proyectos
                .FirstOrDefaultAsync(x => x.codigo == proyecto.codigo && x.estado == "Activo");

            if (db == null)
                return NotFound("No existe ese codigo.");
            db.titulo = proyecto.titulo;
            db.descripcion = proyecto.descripcion;
            db.colaboradores = proyecto.colaboradores;
            db.fechaInicio = proyecto.fechaInicio;
            db.fechaFin = proyecto.fechaFin;
            await context.SaveChangesAsync();
            return Ok($"Se actualizó el codigo: {proyecto.codigo}");
        }
        [HttpDelete("eliminar")]
        public async Task<IActionResult> DeleteProyecto(string codigo)
        {
            var pe = await context.Proyectos
                .FirstOrDefaultAsync(x => x.codigo == codigo && x.estado == "Activo");

            if (pe == null)
                return NotFound("No existe ese codigo para eliminar.");

            pe.estado = "Borrado";
            await context.SaveChangesAsync();
            return Ok($"Se eliminó el Proyecto con codigo: {codigo}");
        }

    }
}