using Microsoft.AspNetCore.Mvc;
using Universidad.Entidades;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/solicitud-produccion")]
    public class SolicitudProduccionController : ControllerBase
    {
        // Simulación de almacenamiento en memoria
        private static List<SolicitudProduccion> solicitudes = new();

        // Obtiene todas las solicitudes
        [HttpGet]
        public IActionResult Listar()
        {
            return Ok(solicitudes);
        }

        // Obtiene una soli por ID
        [HttpGet("{id}")]
        public IActionResult ObtenerPorId(int id)
        {
            var solicitud = solicitudes.FirstOrDefault(s => s.id == id);

            if (solicitud == null)
                return NotFound("Solicitud no encontrada");

            return Ok(solicitud);
        }

        // Coloca una soli
        [HttpPost]
        public IActionResult Registrar(SolicitudProduccion solicitud)
        {
            solicitud.estadopedido = "Pendiente";

            var usuario = User.Identity?.Name ?? "usuario_sistema";

            solicitudes.Add(solicitud);

            return Ok(new
            {
                mensaje = "Solicitud registrada correctamente",
                registradaPor = usuario,
                data = solicitud
            });
        }

        // Actualiza una solicitud dando su ID
        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, SolicitudProduccion solicitudActualizada)
        {
            var solicitud = solicitudes.FirstOrDefault(s => s.id == id);

            if (solicitud == null)
                return NotFound("Solicitud no encontrada");

            solicitud.nombresolicitud = solicitudActualizada.nombresolicitud;
            solicitud.tipocontenido = solicitudActualizada.tipocontenido;
            solicitud.plataforma = solicitudActualizada.plataforma;
            solicitud.fechalimite = solicitudActualizada.fechalimite;
            solicitud.prioridad = solicitudActualizada.prioridad;
            solicitud.estadopedido = solicitudActualizada.estadopedido;

            var usuario = User.Identity?.Name ?? "usuario_sistema";

            return Ok(new
            {
                mensaje = "Solicitud actualizada correctamente",
                actualizadaPor = usuario,
                data = solicitud
            });
        }
    }
}
