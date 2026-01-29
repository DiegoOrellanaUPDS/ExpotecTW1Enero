using Microsoft.AspNetCore.Mvc;
using Universidad.Entidades;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/persona-produccion")]
    public class PersonaProduccionController : ControllerBase
    {
        private static List<PersonaProduccion> personas = new();

        // Conseguir todas las personas
        [HttpGet]
        public IActionResult Listar()
        {
            return Ok(personas);
        }

        // Conseguir persona por ID
        [HttpGet("{id}")]
        public IActionResult ObtenerPorId(int id)
        {
            var persona = personas.FirstOrDefault(p => p.id == id);

            if (persona == null)
                return NotFound("Persona no encontrada");

            return Ok(persona);
        }

        // Añadir nueva persona
        [HttpPost]
        public IActionResult Registrar(PersonaProduccion persona)
        {
            persona.estado = "Activo";

            // Uso de credenciales (si existen)
            var usuario = User.Identity?.Name ?? "usuario_sistema";

            personas.Add(persona);

            return Ok(new
            {
                mensaje = "Persona registrada correctamente",
                registradoPor = usuario,
                data = persona
            });
        }

        //Actualizar persona de produccion
        [HttpPut("{id}")]
        public IActionResult Actualizar(int id, PersonaProduccion personaActualizada)
        {
            var persona = personas.FirstOrDefault(p => p.id == id);

            if (persona == null)
                return NotFound("Persona no encontrada");

            persona.nombre = personaActualizada.nombre;
            persona.apellido = personaActualizada.apellido;
            persona.correo = personaActualizada.correo;
            persona.rol = personaActualizada.rol;
            persona.estado = personaActualizada.estado;

            var usuario = User.Identity?.Name ?? "usuario_sistema";

            return Ok(new
            {
                mensaje = "Persona actualizada correctamente",
                actualizadoPor = usuario,
                data = persona
            });
        }
    }
}
