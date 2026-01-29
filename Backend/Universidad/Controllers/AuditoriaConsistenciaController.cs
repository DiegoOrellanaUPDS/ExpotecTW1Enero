using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entidades;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditoriaController : ControllerBase
    {
        private readonly AppDbContext context;
        public AuditoriaController(AppDbContext context)
        {
            this.context =context;
        }
       [HttpGet]
        public async Task<IActionResult>GetAudit()
        {
            var auditorias = await context.Auditorias
                .OrderByDescending(a => a.fechaDeModificacion)
                .ToListAsync();

            return Ok(auditorias);
        }

        [HttpPost]
    public async Task<ActionResult<Auditoria>> PostAuditoria(Auditoria archivo )
    {
            var xd = await (from ar in context.Auditorias
                            where ar.fechaDeModificacion ==archivo.fechaDeModificacion
                            select ar).FirstOrDefaultAsync();
            if(xd != null)
            {
                return BadRequest("La auditoria con esta fecha ya existe");
            }
        await context.Auditorias.AddAsync(archivo);
        await context.SaveChangesAsync();
        return Ok(archivo);
    }
    }
}