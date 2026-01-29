using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Universidad.Entidades;
using Universidad.Services;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReporteController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly CloudinaryService cloudinaryService;
        public ReporteController(AppDbContext context, CloudinaryService cloudinaryService)
        {
            this.context = context;
            this.cloudinaryService = cloudinaryService;
        }
        [HttpGet("listar")]
        public async Task<IActionResult> GetReportes()
        {
            return Ok(await (from e in context.Reportes
                             where e.estado == "Activo"
                             select e).ToListAsync());
        }
        [HttpGet("buscar")]
        public async Task<IActionResult> GetReporte(string codigo)
        {
            var e = await context.Reportes
                .FirstOrDefaultAsync(x => x.codigo == codigo && x.estado == "Activo");

            if (e == null)
                return NotFound("No se encontró el codigo.");

            return Ok(e);
        }
        [HttpPost("subir")]
        public async Task<IActionResult> SubirReporte(IFormFile archivo, string codigo, string tipoReporte, int idProyecto, string formato)
        {
            if (archivo == null || archivo.Length == 0)
                return BadRequest("Archivo no enviado o está vacío");

            try
            {
                var url = await cloudinaryService.SubirArchivoAsync(archivo);

                if (string.IsNullOrEmpty(url))
                    return BadRequest("Error al subir el archivo a la nube");

                var nuevoReporte = new Reporte
                {
                    codigo = codigo,
                    TipoReporte = tipoReporte,
                    IdProyecto = idProyecto,
                    Formato = formato,
                    UrlArchivo = url,
                    FechaGeneracion = DateOnly.FromDateTime(DateTime.UtcNow),
                    estado = "Activo"
                };

                context.Reportes.Add(nuevoReporte);
                await context.SaveChangesAsync();

                return Ok(nuevoReporte);
            }
            catch (Exception)
            {
                return BadRequest("Error de servidor");
            }
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> ActualizarReporte(IFormFile? archivo, string codigo, string tipoReporte, int idProyecto, string formato)
        {
            // Corregido: Ahora solo busca "Activo" y usa el código correctamente.
            var db = await context.Reportes
                .FirstOrDefaultAsync(x => x.codigo == codigo && x.estado == "Activo");

            if (db == null) return NotFound("No existe el reporte para actualizar");

            try
            {
                // Si hay archivo nuevo, lo sube y reemplaza la URL
                if (archivo != null && archivo.Length > 0)
                {
                    var nuevaUrl = await cloudinaryService.SubirArchivoAsync(archivo);
                    if (!string.IsNullOrEmpty(nuevaUrl))
                    {
                        db.UrlArchivo = nuevaUrl;
                    }
                }

                // Actualiza los demás campos
                db.TipoReporte = tipoReporte;
                db.IdProyecto = idProyecto;
                db.Formato = formato;
                db.FechaGeneracion = DateOnly.FromDateTime(DateTime.UtcNow);

                await context.SaveChangesAsync();

                return Ok(new { mensaje = "Reporte actualizado", datos = db });
            }
            catch (Exception)
            {
                return BadRequest("Error al actualizar el reporte");
            }
        }
        [HttpDelete("eliminar")]
        public async Task<IActionResult> DeleteReporte(string codigo)
        {
            var e = await context.Reportes
                .FirstOrDefaultAsync(x => x.codigo == codigo && x.estado == "Activo");

            if (e == null)
                return NotFound("No existe ese codigo para eliminar.");

            e.estado = "Borrado";
            await context.SaveChangesAsync();

            return Ok($"Se eliminó el Reporte con codigo: {codigo}");
        }
    }
}