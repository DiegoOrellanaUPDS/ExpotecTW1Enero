using Microsoft.AspNetCore.Mvc;
using Universidad.Entidades;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/cancelacion")]
    public class CancelacionController : ControllerBase
    {
        private static List<Cancelacion> cancelaciones = new();

        // Obtiene todas las cancelaciones
        [HttpGet]
        public IActionResult Listar()
        {
            return Ok(cancelaciones);
        }

        // Obtiene todas las cancelaciones x id
        [HttpGet("{id}")]
        public IActionResult ObtenerPorId(int id)
        {
            var cancelacion = cancelaciones.FirstOrDefault(c => c.id == id);

            if (cancelacion == null)
                return NotFound("Cancelación no encontrada");

            return Ok(cancelacion);
        }

        // Añade una nueva cancelación
        [HttpPost]
        public IActionResult RegistrarCancelacion(Cancelacion cancelacion)
        {
            // Fecha automática
            cancelacion.fechacancelacion = DateTime.Now;

            // Usuario que realiza la cancelación
            var usuario = User.Identity?.Name ?? "usuario_sistema";
            cancelacion.autorizadopor = usuario;

            cancelaciones.Add(cancelacion);

            return Ok(new
            {
                mensaje = "Cancelación registrada correctamente",
                data = cancelacion
            });
        }
    }
}
