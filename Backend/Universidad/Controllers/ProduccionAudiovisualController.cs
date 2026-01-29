using Microsoft.AspNetCore.Mvc;
using Universidad.Entidades;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/produccion-audiovisual")]
    public class ProduccionAudiovisualController : ControllerBase
    {
        private static List<ProduccionAudiovisual> producciones = new();

        // Extrae todas las prooducciones
        [HttpGet]
        public IActionResult Listar()
        {
            return Ok(producciones);
        }

        // Extrae todas las prooducciones x ID
        [HttpGet("{id}")]
        public IActionResult ObtenerPorId(int id)
        {
            var produccion = producciones.FirstOrDefault(p => p.id == id);

            if (produccion == null)
                return NotFound("Producción no encontrada");

            return Ok(produccion);
        }

        // Agrega una nueva producción
        [HttpPost]
        public IActionResult IniciarProduccion(ProduccionAudiovisual produccion)
        {
            // Regla de negocio: estado inicial
            produccion.estadoactual = "En producción";
            produccion.fechainicio = DateTime.Now;

            var usuario = User.Identity?.Name ?? "usuario_sistema";

            producciones.Add(produccion);

            return Ok(new
            {
                mensaje = "Producción iniciada correctamente",
                iniciadaPor = usuario,
                data = produccion
            });
        }

        // Actualiza una produccion 
        [HttpPut("{id}")]
        public IActionResult ActualizarProduccion(int id, ProduccionAudiovisual produccionActualizada)
        {
            var produccion = producciones.FirstOrDefault(p => p.id == id);

            if (produccion == null)
                return NotFound("Producción no encontrada");

            produccion.estadoactual = produccionActualizada.estadoactual;
            produccion.responsable = produccionActualizada.responsable;
            produccion.versiondevideo = produccionActualizada.versiondevideo;
            produccion.descripcion = produccionActualizada.descripcion;
            produccion.fechapublicacion = produccionActualizada.fechapublicacion;

            var usuario = User.Identity?.Name ?? "usuario_sistema";

            return Ok(new
            {
                mensaje = "Producción actualizada correctamente",
                actualizadoPor = usuario,
                data = produccion
            });
        }
    }
}
