using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Entidades;

[ApiController]
[Route("api/[controller]")]
public class SistemaController : ControllerBase
{
    private static readonly List<Profesor> _profesores = new()
    {
        new Profesor { Id = 1, Nombre = "Juan Pérez", Categoria = "Titular", Correo = "juan@uni.com", Especialidad = "Matemáticas" },
        new Profesor { Id = 2, Nombre = "María Gómez", Categoria = "Asociado", Correo = "maria@uni.com", Especialidad = "Física" }
    };
    private static int _nextId = 3;

    [HttpGet]
    public ActionResult<IEnumerable<Profesor>> GetProfesores()
    {
        return _profesores;
    }

    [HttpGet("{id}")]
    public ActionResult<Profesor> GetProfesor(int id)
    {
        var profesor = _profesores.FirstOrDefault(p => p.Id == id);
        if (profesor == null)
            return NotFound();
        return profesor;
    }


    [HttpPost]
    public ActionResult<Profesor> PostProfesor(Profesor profesor)
    {
        profesor.Id = _nextId++;
        profesor.FechaCreacion = DateTime.Now;
        _profesores.Add(profesor);
        return CreatedAtAction(nameof(GetProfesor), new { id = profesor.Id }, profesor);
    }

    [HttpPut("{id}")]
    public IActionResult PutProfesor(int id, Profesor profesor)
    {
        if (id != profesor.Id) return BadRequest();
        
        var index = _profesores.FindIndex(p => p.Id == id);
        if (index == -1) return NotFound();
        
        _profesores[index] = profesor;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteProfesor(int id)
    {
        var index = _profesores.FindIndex(p => p.Id == id);
        if (index == -1) return NotFound();
        
        _profesores.RemoveAt(index);
        return NoContent();
    }
}
