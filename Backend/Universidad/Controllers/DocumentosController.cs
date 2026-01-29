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
    public class DocumentosController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly CloudinaryService cloudinaryService;
        public DocumentosController(AppDbContext context,CloudinaryService cloudinaryService)
        {
            this.context=context;
            this.cloudinaryService=cloudinaryService;
        }
        [HttpGet("listar")]
        public async Task<IActionResult> GetDocumentos()
        {
            return Ok(await (from d in context.Documentos
                             where d.estado == "Activo"
                             select d).ToListAsync());
        }
        [HttpGet("buscar")]
        public async Task<IActionResult> GetDocumento(string codigo)
        {
            var d = await context.Documentos
                .FirstOrDefaultAsync(x => x.codigo == codigo && x.estado == "Activo");
        
            if (d == null)
                return NotFound("No se encontró el codigo.");
        
            return Ok(d);
        }
        [HttpPost("subir")]
        public async Task<IActionResult> Subir(IFormFile archivo, string codigo, int nroVersion, int idDocRelacionado)
        {
            if (archivo == null || archivo.Length == 0)
                return BadRequest("Debe seleccionar un archivo.");

            try
            {
                // Subir a Cloudinary
                var url = await cloudinaryService.SubirArchivoAsync(archivo);

                var nuevoDoc = new Documento
                {
                    codigo = codigo,
                    nroVersion = nroVersion,
                    IdDocumento = idDocRelacionado, 
                    url_cloudinary = url,
                    nombreArchivoOriginal = archivo.FileName,
                    tamanoarchivo = archivo.Length,
                    estado = "Activo",
                    fechaPublicada = DateOnly.FromDateTime(DateTime.UtcNow)
                };

                context.Documentos.Add(nuevoDoc);
                await context.SaveChangesAsync();
                return Ok(nuevoDoc);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al subir documento: " + ex.Message);
            }
        }
        [HttpPut("actualizar")]
        public async Task<IActionResult> Actualizar(IFormFile? archivo, string codigo, int nroVersion, int idDocRelacionado)
        {
            var db = await context.Documentos
                .FirstOrDefaultAsync(x => x.codigo == codigo && x.estado == "Activo");

            if (db == null) return NotFound("El documento no existe.");

            try
            {
                if (archivo != null && archivo.Length > 0)
                {
                    var nuevaUrl = await cloudinaryService.SubirArchivoAsync(archivo);
                    db.url_cloudinary = nuevaUrl;
                    db.nombreArchivoOriginal = archivo.FileName;
                    db.tamanoarchivo = archivo.Length;
                }
                db.nroVersion = nroVersion;
                db.IdDocumento = idDocRelacionado;
                db.fechaPublicada = DateOnly.FromDateTime(DateTime.UtcNow);

                await context.SaveChangesAsync();
                return Ok(new { mensaje = "Actualizado correctamente", datos = db });
            }
            catch (Exception ex)
            {
                return BadRequest("Error al actualizar: " + ex.Message);
            }
        }

        [HttpDelete("eliminar")]
        public async Task<IActionResult> DeleteDocumento(string codigo)
        {
            var e = await context.Documentos
                .FirstOrDefaultAsync(x => x.codigo == codigo && x.estado == "Activo");
        
            if (e == null)
                return NotFound("No existe ese codigo para eliminar.");
        
            e.estado = "Borrado";
            await context.SaveChangesAsync();
        
            return Ok($"Se eliminó el Documento con codigo: {codigo}");
        }
    }
}