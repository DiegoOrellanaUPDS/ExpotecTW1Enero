using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Universidad.Entidades;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/servicios-externos")]
    [Authorize]
    public class ServiciosExternosController : ControllerBase
    {
        [HttpGet]
        public IActionResult Listar()
        {
            var servicios = new[]
            {
                new ServicioExterno
                {
                    Id = 1,
                    Nombre = "Plataforma Biblioteca Virtual",
                    Proveedor = "Proveedor Académico",
                    Activo = true
                },
                new ServicioExterno
                {
                    Id = 2,
                    Nombre = "Sistema de Pagos Externos",
                    Proveedor = "Proveedor Financiero",
                    Activo = true
                }
            };

            return Ok(servicios);
        }

        [HttpPost]
        public IActionResult Registrar([FromBody] ServicioExterno servicio)
        {
            servicio.Id = 999;
            return Ok(servicio);
        }
    }
}
