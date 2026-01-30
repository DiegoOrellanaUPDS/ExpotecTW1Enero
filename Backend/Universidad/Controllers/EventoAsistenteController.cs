using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Universidad.Entidades;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/asistente/eventos")]
    public class EventoAsistenteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EventoAsistenteController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/asistente/eventos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventoAsistente>>> GetEventos()
        {
            return await _context.EventosAsistente
                .OrderByDescending(e => e.FechaInicio)
                .ToListAsync();
        }

        // GET: api/asistente/eventos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<EventoAsistente>> GetEvento(int id)
        {
            var evento = await _context.EventosAsistente.FindAsync(id);

            if (evento == null)
                return NotFound(new { mensaje = "Evento no encontrado" });

            return evento;
        }

        // POST: api/asistente/eventos
        [HttpPost]
        public async Task<ActionResult<EventoAsistente>> PostEvento(EventoAsistente evento)
        {
            evento.FechaCreacion = DateTime.Now;
            evento.Estado = "Programado";
            
            _context.EventosAsistente.Add(evento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvento", new { id = evento.Id }, evento);
        }

        // PUT: api/asistente/eventos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvento(int id, EventoAsistente evento)
        {
            if (id != evento.Id)
                return BadRequest(new { mensaje = "ID no coincide" });

            _context.Entry(evento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventoExists(id))
                    return NotFound(new { mensaje = "Evento no encontrado" });
                else
                    throw;
            }

            return Ok(new { mensaje = "Evento actualizado exitosamente" });
        }

        // DELETE: api/asistente/eventos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvento(int id)
        {
            var evento = await _context.EventosAsistente.FindAsync(id);
            if (evento == null)
                return NotFound(new { mensaje = "Evento no encontrado" });

            _context.EventosAsistente.Remove(evento);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Evento eliminado exitosamente" });
        }

        // GET: api/asistente/eventos/proximos
        [HttpGet("proximos")]
        public async Task<ActionResult<IEnumerable<EventoAsistente>>> GetEventosProximos()
        {
            return await _context.EventosAsistente
                .Where(e => e.FechaInicio > DateTime.Now && e.Estado == "Programado")
                .OrderBy(e => e.FechaInicio)
                .ToListAsync();
        }

        // GET: api/asistente/eventos/finalizados
        [HttpGet("finalizados")]
        public async Task<ActionResult<IEnumerable<EventoAsistente>>> GetEventosFinalizados()
        {
            return await _context.EventosAsistente
                .Where(e => e.Estado == "Finalizado")
                .OrderByDescending(e => e.FechaInicio)
                .ToListAsync();
        }

        // PUT: api/asistente/eventos/{id}/cancelar
        [HttpPut("{id}/cancelar")]
        public async Task<IActionResult> CancelarEvento(int id)
        {
            var evento = await _context.EventosAsistente.FindAsync(id);
            
            if (evento == null)
                return NotFound(new { mensaje = "Evento no encontrado" });

            evento.Estado = "Cancelado";
            _context.Entry(evento).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Evento cancelado exitosamente" });
        }

        // PUT: api/asistente/eventos/{id}/finalizar
        [HttpPut("{id}/finalizar")]
        public async Task<IActionResult> FinalizarEvento(int id)
        {
            var evento = await _context.EventosAsistente.FindAsync(id);
            
            if (evento == null)
                return NotFound(new { mensaje = "Evento no encontrado" });

            evento.Estado = "Finalizado";
            _context.Entry(evento).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Evento finalizado exitosamente" });
        }

        private bool EventoExists(int id)
        {
            return _context.EventosAsistente.Any(e => e.Id == id);
        }
    }
}