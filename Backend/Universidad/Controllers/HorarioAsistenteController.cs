using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Universidad.Entidades;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/asistente/horarios")]
    public class HorarioAsistenteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HorarioAsistenteController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/asistente/horarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HorarioAsistente>>> GetHorarios()
        {
            return await _context.HorariosAsistente.ToListAsync();
        }

        // GET: api/asistente/horarios/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<HorarioAsistente>> GetHorario(int id)
        {
            var horario = await _context.HorariosAsistente.FindAsync(id);

            if (horario == null)
                return NotFound(new { mensaje = "Horario no encontrado" });

            return horario;
        }

        // POST: api/asistente/horarios
        [HttpPost]
        public async Task<ActionResult<HorarioAsistente>> PostHorario(HorarioAsistente horario)
        {
            horario.FechaCreacion = DateTime.Now;
            horario.CuposDisponibles = horario.Cupos;
            
            _context.HorariosAsistente.Add(horario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHorario", new { id = horario.Id }, horario);
        }

        // PUT: api/asistente/horarios/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHorario(int id, HorarioAsistente horario)
        {
            if (id != horario.Id)
                return BadRequest(new { mensaje = "ID no coincide" });

            _context.Entry(horario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HorarioExists(id))
                    return NotFound(new { mensaje = "Horario no encontrado" });
                else
                    throw;
            }

            return Ok(new { mensaje = "Horario actualizado exitosamente" });
        }

        // DELETE: api/asistente/horarios/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHorario(int id)
        {
            var horario = await _context.HorariosAsistente.FindAsync(id);
            if (horario == null)
                return NotFound(new { mensaje = "Horario no encontrado" });

            _context.HorariosAsistente.Remove(horario);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Horario eliminado exitosamente" });
        }

        // GET: api/asistente/horarios/disponibles
        [HttpGet("disponibles")]
        public async Task<ActionResult<IEnumerable<HorarioAsistente>>> GetHorariosDisponibles()
        {
            return await _context.HorariosAsistente
                .Where(h => h.CuposDisponibles > 0)
                .ToListAsync();
        }

        // PUT: api/asistente/horarios/{id}/reducir-cupo
        [HttpPut("{id}/reducir-cupo")]
        public async Task<IActionResult> ReducirCupo(int id)
        {
            var horario = await _context.HorariosAsistente.FindAsync(id);
            
            if (horario == null)
                return NotFound(new { mensaje = "Horario no encontrado" });

            if (horario.CuposDisponibles <= 0)
                return BadRequest(new { mensaje = "No hay cupos disponibles" });

            horario.CuposDisponibles--;
            _context.Entry(horario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { 
                mensaje = "Cupo reducido exitosamente", 
                cuposDisponibles = horario.CuposDisponibles 
            });
        }

        // PUT: api/asistente/horarios/{id}/aumentar-cupo
        [HttpPut("{id}/aumentar-cupo")]
        public async Task<IActionResult> AumentarCupo(int id)
        {
            var horario = await _context.HorariosAsistente.FindAsync(id);
            
            if (horario == null)
                return NotFound(new { mensaje = "Horario no encontrado" });

            horario.CuposDisponibles++;
            _context.Entry(horario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { 
                mensaje = "Cupo aumentado exitosamente", 
                cuposDisponibles = horario.CuposDisponibles 
            });
        }

        private bool HorarioExists(int id)
        {
            return _context.HorariosAsistente.Any(e => e.Id == id);
        }
    }
}