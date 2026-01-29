using Microsoft.AspNetCore.Mvc;
using Universidad.Entidades;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Recursos Humanos")]
    public class CargosController : ControllerBase
    {
        [HttpGet]
        public IActionResult Listar()
        {
            return Ok("Listado de cargos");
        }
    }
}
