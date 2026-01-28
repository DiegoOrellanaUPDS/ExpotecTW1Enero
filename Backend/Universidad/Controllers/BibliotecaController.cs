using Microsoft.AspNetCore.Mvc;
using Entidades;
using System.Collections.Generic;

namespace universidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BibliotecaController : ControllerBase
    {
        private static List<Libros> listaLibros = new List<Libros>();

        [HttpGet("libros")]
        public IActionResult ListarLibros()
        {
            return Ok(listaLibros);
        }

        [HttpPost("libros")]
        public IActionResult RegistrarLibro([FromBody] Libros libro)
        {
            if (libro == null)
                return BadRequest("Datos inválidos");

            listaLibros.Add(libro);

            return Ok(new { mensaje = "Libro registrado", datos = libro });
        }
    }
}
