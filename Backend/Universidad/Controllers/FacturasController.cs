using Data;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Caja.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacturaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FacturaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/factura
        [HttpGet]
        public async Task<ActionResult<List<Factura>>> GetFacturas()
        {
            return Ok(await _context.Facturas
                .AsNoTracking()
                .Where(f => f.Estado)
                .ToListAsync());
        }

        // GET: api/factura/{codigo}
        [HttpGet("{codigo}")]
        public async Task<ActionResult<Factura>> GetFactura(string codigo)
        {
            var factura = await _context.Facturas
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.codigo == codigo && f.Estado);

            if (factura == null)
                return NotFound();

            return Ok(factura);
        }

        // GET: api/factura/borrados
        [HttpGet("borrados")]
        public async Task<ActionResult<List<Factura>>> GetFacturasBorradas()
        {
            return Ok(await _context.Facturas
                .AsNoTracking()
                .Where(f => !f.Estado)
                .ToListAsync());
        }

        // POST: api/factura
        [HttpPost]
        public async Task<ActionResult<Factura>> PostFactura(
            string codigo,
            string codigo_producto,
            string codigo_cliente,
            string? codigo_beca,
            int? nit_receptor,
            int cantidad,
            string descripcion,
            decimal subtotal,
            decimal descuento
        )
        {
            if (string.IsNullOrWhiteSpace(codigo) ||
                string.IsNullOrWhiteSpace(codigo_producto) ||
                string.IsNullOrWhiteSpace(codigo_cliente) ||
                string.IsNullOrWhiteSpace(descripcion))
                return BadRequest("Datos obligatorios incompletos");

            if (cantidad <= 0 || subtotal <= 0)
                return BadRequest("Cantidad y subtotal inválidos");

            // FK Producto
            if (!await _context.Productos.AnyAsync(p =>
                p.codigo == codigo_producto && p.estado))
                return BadRequest("Producto inexistente o inactivo");

            // FK Cliente
            if (!await _context.Clientes.AnyAsync(c =>
                c.codigo == codigo_cliente && c.Estado))
                return BadRequest("Cliente inexistente o inactivo");

            // Código único
            if (await _context.Facturas.AnyAsync(f => f.codigo == codigo))
                return BadRequest("Ya existe una factura con ese código");

            var montoFinal = subtotal - descuento;
            if (montoFinal < 0) montoFinal = 0;

            var factura = new Factura
            {
                codigo = codigo,
                codigo_producto = codigo_producto,
                codigo_cliente = codigo_cliente,
                codigo_beca = codigo_beca,      // opcional
                nit_receptor = nit_receptor,    // opcional
                cantidad = cantidad,
                descripcion = descripcion,
                subtotal = subtotal,
                descuento = descuento,
                monto_final = montoFinal,
                fecha_emision = DateTime.Now,
                Estado = true
            };

            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFactura),
                new { codigo = factura.codigo }, factura);
        }

        // PUT: api/factura/{codigo}
        [HttpPut("{codigo}")]
        public async Task<ActionResult<Factura>> PutFactura(
            string codigo,
            string codigo_producto,
            string codigo_cliente,
            string? codigo_beca,
            int? nit_receptor,
            int cantidad,
            string descripcion,
            decimal subtotal,
            decimal descuento
        )
        {
            var factura = await _context.Facturas
                .FirstOrDefaultAsync(f => f.codigo == codigo && f.Estado);

            if (factura == null)
                return NotFound();

            if (cantidad <= 0 || subtotal <= 0)
                return BadRequest("Cantidad o subtotal inválidos");

            // FK Producto
            if (!await _context.Productos.AnyAsync(p =>
                p.codigo == codigo_producto && p.estado))
                return BadRequest("Producto inexistente o inactivo");

            // FK Cliente
            if (!await _context.Clientes.AnyAsync(c =>
                c.codigo == codigo_cliente && c.Estado))
                return BadRequest("Cliente inexistente o inactivo");

            factura.codigo_producto = codigo_producto;
            factura.codigo_cliente = codigo_cliente;
            factura.codigo_beca = codigo_beca;
            factura.nit_receptor = nit_receptor;
            factura.cantidad = cantidad;
            factura.descripcion = descripcion;
            factura.subtotal = subtotal;
            factura.descuento = descuento;
            factura.monto_final = subtotal - descuento;

            if (factura.monto_final < 0)
                factura.monto_final = 0;

            await _context.SaveChangesAsync();

            return Ok(factura);
        }

        // DELETE: api/factura/{codigo}
        [HttpDelete("{codigo}")]
        public async Task<ActionResult<Factura>> DeleteFactura(string codigo)
        {
            var factura = await _context.Facturas
                .FirstOrDefaultAsync(f => f.codigo == codigo && f.Estado);

            if (factura == null)
                return NotFound();

            factura.Estado = false;
            await _context.SaveChangesAsync();

            return Ok(factura);
        }

        // PUT: api/factura/habilitar/{codigo}
        [HttpPut("habilitar/{codigo}")]
        public async Task<ActionResult<Factura>> HabilitarFactura(string codigo)
        {
            var factura = await _context.Facturas
                .FirstOrDefaultAsync(f => f.codigo == codigo && !f.Estado);

            if (factura == null)
                return NotFound();

            factura.Estado = true;
            await _context.SaveChangesAsync();

            return Ok(factura);
        }
    }
}
