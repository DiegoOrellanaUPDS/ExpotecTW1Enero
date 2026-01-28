using Data;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Caja.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/producto
        [HttpGet]
        public async Task<ActionResult<List<Producto>>> GetProductos()
        {
            var productos = await _context.Productos
                .AsNoTracking()
                .Where(p => p.estado == true)
                .ToListAsync();

            return Ok(productos);
        }

        // GET: api/producto/{codigo}
        [HttpGet("{codigo}")]
        public async Task<ActionResult<Producto>> GetProducto(string codigo)
        {
            var producto = await _context.Productos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.codigo == codigo && p.estado == true);

            if (producto == null)
                return NotFound();

            return Ok(producto);
        }

        // GET: api/producto/borrados
        [HttpGet("borrados")]
        public async Task<ActionResult<List<Producto>>> GetProductosBorrados()
        {
            var productos = await _context.Productos
                .AsNoTracking()
                .Where(p => p.estado == false)
                .ToListAsync();

            return Ok(productos);
        }

        // POST: api/producto
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(
            string codigo,
            string nombre,
            decimal precio_unitario
        )
        {
            if (string.IsNullOrWhiteSpace(codigo) || string.IsNullOrWhiteSpace(nombre))
                return BadRequest("Código y nombre son obligatorios");

            if (precio_unitario <= 0)
                return BadRequest("El precio unitario debe ser mayor a 0");

            var existe = await _context.Productos.AnyAsync(p => p.codigo == codigo);
            if (existe)
                return BadRequest("Ya existe un producto con ese código");

            var producto = new Producto
            {
                codigo = codigo,
                nombre = nombre,
                precio_unitario = precio_unitario,
                estado = true
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetProducto),
                new { codigo = producto.codigo },
                producto
            );
        }

        // PUT: api/producto/{codigo}
        [HttpPut("{codigo}")]
        public async Task<ActionResult<Producto>> PutProducto(
            string codigo,
            string codigoNuevo,
            string nombre,
            decimal precio_unitario
        )
        {
            if (string.IsNullOrWhiteSpace(codigo) ||
                string.IsNullOrWhiteSpace(codigoNuevo) ||
                string.IsNullOrWhiteSpace(nombre))
                return BadRequest("Datos inválidos");

            if (precio_unitario <= 0)
                return BadRequest("El precio unitario debe ser mayor a 0");

            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.codigo == codigo && p.estado == true);

            if (producto == null)
                return NotFound();

            producto.codigo = codigoNuevo;
            producto.nombre = nombre;
            producto.precio_unitario = precio_unitario;

            await _context.SaveChangesAsync();

            return Ok(producto);
        }

        // DELETE: api/producto/{codigo}
        [HttpDelete("{codigo}")]
        public async Task<ActionResult<Producto>> DeleteProducto(string codigo)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.codigo == codigo && p.estado == true);

            if (producto == null)
                return NotFound();

            producto.estado = false;
            await _context.SaveChangesAsync();

            return Ok(producto);
        }

        // PUT: api/producto/habilitar/{codigo}
        [HttpPut("habilitar/{codigo}")]
        public async Task<ActionResult<Producto>> HabilitarProducto(string codigo)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.codigo == codigo && p.estado == false);

            if (producto == null)
                return NotFound();

            producto.estado = true;
            await _context.SaveChangesAsync();

            return Ok(producto);
        }
    }
}
