using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Universidad.Data;
using Universidad.Modules.Vicerrectorado.Dtos;
using Universidad.Modules.Vicerrectorado.Models;

namespace Universidad.Modules.Vicerrectorado.Controllers;

[ApiController]
[Route("api/v1/vicerrectorado")]
[Authorize]
public class VicerrectoradoController : ControllerBase
{
    private readonly AppDbContext _db;

    public VicerrectoradoController(AppDbContext db)
    {
        _db = db;
    }

    private string GetUserId()
        => User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? User.FindFirstValue("sub")
        ?? User.FindFirstValue("preferred_username")
        ?? "unknown";

    [HttpGet("me")]
    public IActionResult Me()
        => Ok(new { userId = GetUserId() });

    // ===== Vicerrectores =====
    [HttpGet("vicerrectores")]
    public async Task<IActionResult> ListarVicerrectores()
        => Ok(await _db.Vicerrectores.OrderByDescending(x => x.Activo).ThenBy(x => x.NombreCompleto).ToListAsync());

    [HttpPost("vicerrectores")]
    public async Task<IActionResult> CrearVicerrector([FromBody] Vicerrector model)
    {
        _db.Vicerrectores.Add(model);
        await _db.SaveChangesAsync();
        return Ok(model);
    }

    // ===== Convocatorias =====
    [HttpGet("convocatorias")]
    public async Task<IActionResult> ListarConvocatorias()
        => Ok(await _db.VicerrectoradoConvocatorias.OrderByDescending(x => x.FechaPublicacion).ToListAsync());

    [HttpPost("convocatorias")]
    public async Task<IActionResult> CrearConvocatoria([FromBody] CrearConvocatoriaDto dto)
    {
        var c = new Convocatoria
        {
            Titulo = dto.Titulo,
            Descripcion = dto.Descripcion,
            CreadoPorUserId = GetUserId(),
            FechaPublicacion = DateTime.UtcNow,
            Estado = EstadoConvocatoria.Borrador
        };

        _db.VicerrectoradoConvocatorias.Add(c);
        await _db.SaveChangesAsync();
        return Ok(c);
    }

    [HttpPatch("convocatorias/{id:guid}/estado")]
    public async Task<IActionResult> CambiarEstado(Guid id, [FromBody] ActualizarEstadoConvocatoriaDto dto)
    {
        var item = await _db.VicerrectoradoConvocatorias.FindAsync(id);
        if (item is null) return NotFound();

        item.Estado = dto.Estado;

        if (dto.Estado == EstadoConvocatoria.Cerrada)
            item.FechaCierre = dto.FechaCierre ?? DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok(item);
    }

    // ===== Trámites =====
    [HttpGet("tramites")]
    public async Task<IActionResult> ListarTramites()
        => Ok(await _db.VicerrectoradoTramites.OrderByDescending(x => x.FechaSolicitud).ToListAsync());

    [HttpPost("tramites")]
    public async Task<IActionResult> CrearTramite([FromBody] CrearTramiteDto dto)
    {
        var t = new TramiteVicerrectorado
        {
            Tipo = dto.Tipo,
            Descripcion = dto.Descripcion,
            SolicitanteUserId = GetUserId(),
            Estado = EstadoTramite.Registrado,
            FechaSolicitud = DateTime.UtcNow
        };

        _db.VicerrectoradoTramites.Add(t);
        await _db.SaveChangesAsync();
        return Ok(t);
    }

    [HttpPatch("tramites/{id:guid}")]
    public async Task<IActionResult> ActualizarTramite(Guid id, [FromBody] ActualizarTramiteDto dto)
    {
        var item = await _db.VicerrectoradoTramites.FindAsync(id);
        if (item is null) return NotFound();

        item.Estado = dto.Estado;
        item.Observacion = dto.Observacion;

        await _db.SaveChangesAsync();
        return Ok(item);
    }
}
