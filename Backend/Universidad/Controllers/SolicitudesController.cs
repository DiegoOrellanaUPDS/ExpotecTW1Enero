using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitudesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SolicitudesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Solicitudes
        // Muestra todas las solicitudes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Solicitud>>> GetSolicitudes()
        {
            return await _context.Solicitudes.ToListAsync();
        }

        // GET: api/Solicitudes/buscar?codigo=SOL001&tipo=Trámite académico
        // Filtra por código y tipo
        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<Solicitud>>> GetSolicitudesPorCodigoTipo(
            [FromQuery] string codigo,
            [FromQuery] string tipo)
        {
            var solicitudes = await _context.Solicitudes
                .Where(s => s.CodigoSolicitud == codigo && s.Tipo == tipo)
                .ToListAsync();

            if (solicitudes == null || solicitudes.Count == 0)
            {
                return NotFound();
            }

            return solicitudes;
        }

        // POST: api/Solicitudes
        // Inserta una nueva solicitud
        [HttpPost]
        public async Task<ActionResult<Solicitud>> PostSolicitud(Solicitud solicitud)
        {
            _context.Solicitudes.Add(solicitud);
            await _context.SaveChangesAsync();

            // Devuelve la solicitud creada con su Id asignado
            return CreatedAtAction(nameof(GetSolicitudes), new { id = solicitud.Id }, solicitud);
        }
    }
}

