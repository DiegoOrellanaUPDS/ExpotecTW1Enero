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
    public class VersionesDocumentosController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly CloudinaryService cloudinaryService;

        public VersionesDocumentosController(AppDbContext context, CloudinaryService cloudinaryService)
        {
            this.context = context;
            this.cloudinaryService = cloudinaryService;
        }
        [HttpGet("listar")]
        public async Task<IActionResult> GetVersionDocumentos()
        {
            return Ok(await (from e in context.VersionDocumentos
                             where e.estado == "Activo"
                             select e).ToListAsync());
        }
        [HttpGet("buscar")]
        public async Task<IActionResult> GetReporte(string codigo)
        {
            var e = await context.VersionDocumentos
                .FirstOrDefaultAsync(x => x.codigo == codigo && x.estado == "Activo");

            if (e == null)
                return NotFound("No se encontró el codigo.");

            return Ok(e);
        }
        [HttpPost("subir")]
        public async Task<IActionResult> Subir(IFormFile archivo, string codigo, int idDocumento, int nroVersion)
        {
            if (archivo == null || archivo.Length == 0)
                return BadRequest("Archivo no enviado o está vacío");

            try
            {
                var url = await cloudinaryService.SubirArchivoAsync(archivo);

                if (string.IsNullOrEmpty(url))
                    return BadRequest("Error al subir el archivo a la nube");
                var nuevaVersion = new VersionDocumento
                {
                    codigo = codigo,
                    nroVersion = nroVersion,
                    IdDocumento = idDocumento,
                    url_cloudinary = url,
                    NombreArchivoOriginal = archivo.FileName,
                    tamanoarchivo = archivo.Length,
                    estado = "Activo",
                    fechaPublicada = DateOnly.FromDateTime(DateTime.UtcNow)
                };
                await context.VersionDocumentos.AddAsync(nuevaVersion);
                await context.SaveChangesAsync();
                return Ok(nuevaVersion);
            }
            catch (Exception )
            {
                return BadRequest("Error de servidor");
            }
        }
        [HttpPut("actualizar")]
        public async Task<IActionResult> Actualizar(IFormFile? archivo, string codigo, int nroVersion, int idDocumento)
        {
            var db = await context.VersionDocumentos
                .FirstOrDefaultAsync(v => v.codigo == codigo && v.estado == "Activo");

            if (db == null) return NotFound("No existe la versión para actualizar");

            try
            {
                if (archivo != null && archivo.Length > 0)
                {
                    var nuevaUrl = await cloudinaryService.SubirArchivoAsync(archivo);
                    if (!string.IsNullOrEmpty(nuevaUrl))
                    {
                        db.url_cloudinary = nuevaUrl;
                        db.NombreArchivoOriginal = archivo.FileName;
                        db.tamanoarchivo = archivo.Length;
                    }
                }

                db.nroVersion = nroVersion;
                db.IdDocumento = idDocumento;
                db.fechaPublicada = DateOnly.FromDateTime(DateTime.UtcNow);
                await context.SaveChangesAsync();
                return Ok(new { mensaje = "Versión actualizada", datos = db });
            }
            catch (Exception)
            {
                return BadRequest("Error al actualizar la versión");
            }
        }
        [HttpDelete("eliminar")]
        public async Task<IActionResult> DeleteReporte(string codigo)
        {
            var e = await context.VersionDocumentos
                .FirstOrDefaultAsync(x => x.codigo == codigo && x.estado == "Activo");

            if (e == null)
                return NotFound("No existe ese codigo para eliminar.");

            e.estado = "Borrado";
            await context.SaveChangesAsync();

            return Ok($"Se eliminó el Reporte con codigo: {codigo}");
        }
    }
}