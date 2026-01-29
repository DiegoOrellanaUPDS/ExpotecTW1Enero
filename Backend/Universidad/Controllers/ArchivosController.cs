using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ArchivosController : ControllerBase
    {
        [HttpGet]
        public IActionResult Listar()
        {
            return Ok("Listado de archivos");
        }

        [HttpPost]
        public IActionResult Subir()
        {
            return Ok("Archivo subido correctamente");
        }
    }
}
