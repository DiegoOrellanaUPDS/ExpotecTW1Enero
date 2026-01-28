using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Entidades;

[ApiController]
[Route("api/[controller]")]
public class SistemaController : ControllerBase
{
    private readonly AppDbContext _context;

    public SistemaController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Profesor>>> GetProfesores()
    {
        return await _context.Profesores.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Profesor>> GetProfesor(int id)
    {
        var profesor = await _context.Profesores.FindAsync(id);
        
        if (profesor == null)
            return NotFound();
            
        return profesor;
    }

    [HttpPost]
    public async Task<ActionResult<Profesor>> PostProfesor(Profesor profesor)
    {
        _context.Profesores.Add(profesor);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetProfesor), 
            new { id = profesor.Id }, profesor);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutProfesor(int id, Profesor profesor)
    {
        if (id != profesor.Id)
            return BadRequest();
            
        _context.Entry(profesor).State = EntityState.Modified;
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProfesorExists(id))
                return NotFound();
            throw;
        }
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProfesor(int id)
    {
        var profesor = await _context.Profesores.FindAsync(id);
        if (profesor == null)
            return NotFound();
            
        _context.Profesores.Remove(profesor);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }

    private bool ProfesorExists(int id)
    {
        return _context.Profesores.Any(e => e.Id == id);
    }
}
