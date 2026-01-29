using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TramitesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Listar()
        {
            return Ok("Listado de trámites");
        }

        [HttpPost]
        public IActionResult Registrar()
        {
            return Ok("Trámite registrado correctamente");
        }
    }
}
