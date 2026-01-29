using Microsoft.AspNetCore.Mvc;
using Universidad.Entidades;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Recursos Humanos")]
    public class EmpleadosController : ControllerBase
    {
        // Simulación de base de datos en memoria
        private static List<Empleado> empleados = new List<Empleado>();

        // GET: api/Empleados
        [HttpGet]
        public IActionResult Listar()
        {
            return Ok(empleados);
        }

        // POST: api/Empleados
        [HttpPost]
        public IActionResult Crear([FromBody] Empleado empleado)
        {
            empleado.Id = empleados.Count + 1;
            empleados.Add(empleado);

            return Ok(empleado);
        }

        // DELETE: api/Empleados/{id}
        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            var empleado = empleados.FirstOrDefault(e => e.Id == id);

            if (empleado == null)
            {
                return NotFound($"No existe un empleado con id {id}");
            }

            empleados.Remove(empleado);
            return Ok($"Empleado con id {id} eliminado correctamente");
        }
    }
}

