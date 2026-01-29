using Entidades;
using Microsoft.AspNetCore.Mvc;
using Universidad.Entidades;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoordinacionController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] Carrera carrera)
        {
            if (carrera == null) return BadRequest("El modelo no puede ser nulo.");
            return Ok(new { mensaje = "Coordinación Académica: Registro exitoso", data = carrera });
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] UsuarioCoordinacion login)
        {       
            if (login == null || string.IsNullOrEmpty(login.NombreUsuario))
            {
                return BadRequest("Datos de acceso incompletos.");
            }

            // Aquí es donde el sistema validará con OAuth más adelante
            return Ok(new { 
                mensaje = "Login exitoso para Coordinación Académica", 
                usuario = login.NombreUsuario,
                token_simulado = "auth_token_beymar_2024" 
            });
        }
    }

}
