using Microsoft.AspNetCore.Mvc;
using Universidad.Entidades;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/historial-cambios")]
    public class HistorialCambiosController : ControllerBase
    {
        private static List<HistorialCambios> historial = new();

        // GET: api/historial-cambios
        [HttpGet]
        public IActionResult Listar()
        {
            return Ok(historial);
        }

        // GET: api/historial-cambios/{id}
        [HttpGet("{id}")]
        public IActionResult ObtenerPorId(int id)
        {
            var cambio = historial.FirstOrDefault(h => h.id == id);

            if (cambio == null)
                return NotFound("Cambio no encontrado");

            return Ok(cambio);
        }

        // POST: api/historial-cambios
        [HttpPost]
        public IActionResult RegistrarCambio(HistorialCambios cambio)
        {
            // Fecha automática del cambio
            cambio.fechacambio = DateTime.Now;

            var usuario = User.Identity?.Name ?? "usuario_sistema";
            cambio.realizadopor = usuario;

            historial.Add(cambio);

            return Ok(new
            {
                mensaje = "Cambio registrado correctamente",
                data = cambio
            });
        }
    }
}
