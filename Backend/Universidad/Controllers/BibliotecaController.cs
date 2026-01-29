using Microsoft.AspNetCore.Mvc;
using Data;
using Entidades;
using System.Collections.Generic;

namespace universidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BibliotecaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BibliotecaController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // 📚 LIBROS
        // =========================

        [HttpGet("libros")]
        public IActionResult GetLibros()
        {
            return Ok(_context.Libros.ToList());
        }

        [HttpPost("libros")]
        public IActionResult CrearLibro([FromBody] Libros libro)
        {
            if (libro == null)
                return BadRequest();

            _context.Libros.Add(libro);
            _context.SaveChanges();
            return Ok(libro);
        }

        [HttpPut("libros/{id}")]
        public IActionResult EditarLibro(int id, [FromBody] Libros libro)
        {
            var existente = _context.Libros.Find(id);
            if (existente == null) return NotFound();

            existente.titulo = libro.titulo;
            existente.autor = libro.autor;
            existente.aniopublicacion = libro.aniopublicacion;
            existente.estado = libro.estado;

            _context.SaveChanges();
            return Ok(existente);
        }

        // =========================
        // 🗂 CATEGORÍAS
        // =========================

        [HttpGet("categorias")]
        public IActionResult GetCategorias()
        {
            return Ok(_context.Categorias.ToList());
        }

        [HttpPost("categorias")]
        public IActionResult CrearCategoria([FromBody] Categorias categoria)
        {
            if (categoria == null)
                return BadRequest();

            _context.Categorias.Add(categoria);
            _context.SaveChanges();
            return Ok(categoria);
        }

        [HttpPut("categorias/{id}")]
        public IActionResult EditarCategoria(int id, [FromBody] Categorias categoria)
        {
            var existente = _context.Categorias.Find(id);
            if (existente == null) return NotFound();

            existente.nombre = categoria.nombre;
            existente.descripcion = categoria.descripcion;
            existente.estado = categoria.estado;

            _context.SaveChanges();
            return Ok(existente);
        }

        // =========================
        // 🔄 PRÉSTAMOS
        // =========================

        [HttpGet("prestamos")]
        public IActionResult GetPrestamos()
        {
            return Ok(_context.Prestamos.ToList());
        }

        [HttpPost("prestamos")]
        public IActionResult CrearPrestamo([FromBody] Prestamos prestamo)
        {
            if (prestamo == null)
                return BadRequest();

            _context.Prestamos.Add(prestamo);
            _context.SaveChanges();
            return Ok(prestamo);
        }

        [HttpPut("prestamos/{id}")]
        public IActionResult EditarPrestamo(int id, [FromBody] Prestamos prestamo)
        {
            var existente = _context.Prestamos.Find(id);
            if (existente == null) return NotFound();

            existente.idlibro = prestamo.idlibro;
            existente.idestudiante = prestamo.idestudiante;
            existente.fechaprestamo = prestamo.fechaprestamo;
            existente.fechadevolucion = prestamo.fechadevolucion;
            existente.estado = prestamo.estado;

            _context.SaveChanges();
            return Ok(existente);
        }
    }
}
