using Microsoft.AspNetCore.Mvc;
using Universidad.Entidades;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Recursos Humanos")]
    public class EmpleadosController : ControllerBase
    {
        [HttpGet]
        public IActionResult Listar()
        {
            return Ok("Listado de empleados - RRHH");
        }

        [HttpPost]
        public IActionResult Crear([FromBody] Empleado empleado)
        {
            return Ok(empleado);
        }
    }
}

