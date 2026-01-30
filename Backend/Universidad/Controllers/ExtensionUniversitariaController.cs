using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entidades;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExtensionController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        public ExtensionController(AppDbContext context)
        {
            _context = context;
        }
        
        // ========== PROYECTOS DE EXTENSIÓN ==========
        
        [HttpGet("proyectos")]
        public async Task<IActionResult> GetProyectos()
        {
            var proyectos = await (from p in _context.ProyectoExtensions 
                                   where p.Estado != "eliminado" 
                                   select p).ToListAsync();
            return Ok(proyectos);
        }
        
        [HttpGet("proyectos/{id}")]
        public async Task<IActionResult> GetProyecto(int id)
        {
            var proyecto = await (from p in _context.ProyectoExtensions 
                                  where p.Id == id && p.Estado != "eliminado" 
                                  select p).FirstOrDefaultAsync();
            
            if (proyecto == null) 
                return NotFound("No se encontró el proyecto");
            
            return Ok(proyecto);
        }
        
        [HttpPost("proyectos")]
        public async Task<IActionResult> PostProyecto(ProyectoExtension proyecto)
        {
            proyecto.CodigoProyecto = await GenerarCodigoProyecto();
            proyecto.FechaCreacion = DateOnly.FromDateTime(DateTime.Now);
            proyecto.Estado = "planificado";
            
            await _context.ProyectoExtensions.AddAsync(proyecto);
            await _context.SaveChangesAsync();
            
            return Ok(new { 
                mensaje = "Proyecto creado correctamente", 
                codigo = proyecto.CodigoProyecto,
                id = proyecto.Id 
            });
        }
        
        [HttpPut("proyectos/{id}")]
        public async Task<IActionResult> PutProyecto(int id, ProyectoExtension proyectoActualizado)
        {
            var proyecto = await (from p in _context.ProyectoExtensions 
                                  where p.Id == id && p.Estado != "eliminado" 
                                  select p).FirstOrDefaultAsync();
            
            if (proyecto == null) 
                return NotFound("No se encontró el proyecto");
            
            proyecto.Titulo = proyectoActualizado.Titulo;
            proyecto.Descripcion = proyectoActualizado.Descripcion;
            proyecto.Responsable = proyectoActualizado.Responsable;
            proyecto.FechaInicio = proyectoActualizado.FechaInicio;
            proyecto.FechaFin = proyectoActualizado.FechaFin;
            proyecto.Presupuesto = proyectoActualizado.Presupuesto;
            proyecto.Beneficiarios = proyectoActualizado.Beneficiarios;
            
            await _context.SaveChangesAsync();
            
            return Ok(new { mensaje = "Proyecto actualizado correctamente" });
        }
        
        [HttpDelete("proyectos/{id}")]
        public async Task<IActionResult> DeleteProyecto(int id)
        {
            var proyecto = await (from p in _context.ProyectoExtensions 
                                  where p.Id == id 
                                  select p).FirstOrDefaultAsync();
            
            if (proyecto == null) 
                return NotFound("No se encontró el proyecto");
            
            proyecto.Estado = "eliminado";
            await _context.SaveChangesAsync();
            
            return Ok(new { mensaje = "Proyecto eliminado correctamente" });
        }
        
        // ========== ACTIVIDADES DE EXTENSIÓN ==========
        
        [HttpGet("actividades")]
        public async Task<IActionResult> GetActividades()
        {
            var actividades = await (from a in _context.ActividadExtensions 
                                     where a.Estado != "cancelada" 
                                     orderby a.FechaActividad 
                                     select a).ToListAsync();
            return Ok(actividades);
        }
        
        [HttpGet("actividades/{id}")]
        public async Task<IActionResult> GetActividad(int id)
        {
            var actividad = await (from a in _context.ActividadExtensions 
                                   where a.Id == id 
                                   select a).FirstOrDefaultAsync();
            
            if (actividad == null) 
                return NotFound("No se encontró la actividad");
            
            return Ok(actividad);
        }
        
        [HttpPost("actividades")]
        public async Task<IActionResult> PostActividad(ActividadExtension actividad)
        {
            actividad.CodigoActividad = await GenerarCodigoActividad();
            actividad.FechaCreacion = DateOnly.FromDateTime(DateTime.Now);
            actividad.Estado = "planificada";
            
            await _context.ActividadExtensions.AddAsync(actividad);
            await _context.SaveChangesAsync();
            
            return Ok(new { 
                mensaje = "Actividad creada correctamente", 
                codigo = actividad.CodigoActividad,
                id = actividad.Id 
            });
        }
        
        [HttpPut("actividades/{id}")]
        public async Task<IActionResult> PutActividad(int id, ActividadExtension actividadActualizada)
        {
            var actividad = await (from a in _context.ActividadExtensions 
                                   where a.Id == id 
                                   select a).FirstOrDefaultAsync();
            
            if (actividad == null) 
                return NotFound("No se encontró la actividad");
            
            actividad.Titulo = actividadActualizada.Titulo;
            actividad.Descripcion = actividadActualizada.Descripcion;
            actividad.TipoActividad = actividadActualizada.TipoActividad;
            actividad.FechaActividad = actividadActualizada.FechaActividad;
            actividad.Lugar = actividadActualizada.Lugar;
            actividad.Participantes = actividadActualizada.Participantes;
            actividad.Responsable = actividadActualizada.Responsable;
            
            await _context.SaveChangesAsync();
            
            return Ok(new { mensaje = "Actividad actualizada correctamente" });
        }
        
        [HttpDelete("actividades/{id}")]
        public async Task<IActionResult> DeleteActividad(int id)
        {
            var actividad = await (from a in _context.ActividadExtensions 
                                   where a.Id == id 
                                   select a).FirstOrDefaultAsync();
            
            if (actividad == null) 
                return NotFound("No se encontró la actividad");
            
            actividad.Estado = "cancelada";
            await _context.SaveChangesAsync();
            
            return Ok(new { mensaje = "Actividad cancelada correctamente" });
        }
        
        // ========== VOLUNTARIOS ==========
        
        [HttpGet("voluntarios")]
        public async Task<IActionResult> GetVoluntarios()
        {
            var voluntarios = await (from v in _context.VoluntarioExtensions 
                                     where v.Estado != "inactivo" 
                                     select v).ToListAsync();
            return Ok(voluntarios);
        }
        
        [HttpGet("voluntarios/{id}")]
        public async Task<IActionResult> GetVoluntario(int id)
        {
            var voluntario = await (from v in _context.VoluntarioExtensions 
                                    where v.Id == id && v.Estado != "inactivo" 
                                    select v).FirstOrDefaultAsync();
            
            if (voluntario == null) 
                return NotFound("No se encontró el voluntario");
            
            return Ok(voluntario);
        }
        
        [HttpPost("voluntarios")]
        public async Task<IActionResult> PostVoluntario(VoluntarioExtension voluntario)
        {
            voluntario.FechaRegistro = DateOnly.FromDateTime(DateTime.Now);
            voluntario.Estado = "activo";
            
            await _context.VoluntarioExtensions.AddAsync(voluntario);
            await _context.SaveChangesAsync();
            
            return Ok(new { 
                mensaje = "Voluntario registrado correctamente", 
                id = voluntario.Id 
            });
        }
        
        [HttpPut("voluntarios/{id}")]
        public async Task<IActionResult> PutVoluntario(int id, VoluntarioExtension voluntarioActualizado)
        {
            var voluntario = await (from v in _context.VoluntarioExtensions 
                                    where v.Id == id && v.Estado != "inactivo" 
                                    select v).FirstOrDefaultAsync();
            
            if (voluntario == null) 
                return NotFound("No se encontró el voluntario");
            
            voluntario.NombreCompleto = voluntarioActualizado.NombreCompleto;
            voluntario.Correo = voluntarioActualizado.Correo;
            voluntario.Telefono = voluntarioActualizado.Telefono;
            voluntario.AreaInteres = voluntarioActualizado.AreaInteres;
            voluntario.HorasVoluntariado = voluntarioActualizado.HorasVoluntariado;
            
            await _context.SaveChangesAsync();
            
            return Ok(new { mensaje = "Voluntario actualizado correctamente" });
        }
        
        [HttpDelete("voluntarios/{id}")]
        public async Task<IActionResult> DeleteVoluntario(int id)
        {
            var voluntario = await (from v in _context.VoluntarioExtensions 
                                    where v.Id == id && v.Estado != "inactivo" 
                                    select v).FirstOrDefaultAsync();
            
            if (voluntario == null) 
                return NotFound("No se encontró el voluntario");
            
            voluntario.Estado = "inactivo";
            await _context.SaveChangesAsync();
            
            return Ok(new { mensaje = "Voluntario desactivado correctamente" });
        }
        
        // ========== SOCIOS COMUNITARIOS ==========
        
        [HttpGet("socios")]
        public async Task<IActionResult> GetSocios()
        {
            var socios = await (from s in _context.SocioExtensionExtensions 
                                where s.Estado != "inactivo" 
                                select s).ToListAsync();
            return Ok(socios);
        }
        
        [HttpGet("socios/{id}")]
        public async Task<IActionResult> GetSocio(int id)
        {
            var socio = await (from s in _context.SocioExtensionExtensions 
                               where s.Id == id && s.Estado != "inactivo" 
                               select s).FirstOrDefaultAsync();
            
            if (socio == null) 
                return NotFound("No se encontró el socio");
            
            return Ok(socio);
        }
        
        [HttpPost("socios")]
        public async Task<IActionResult> PostSocio(SocioExtension socio)
        {
            socio.FechaRegistro = DateOnly.FromDateTime(DateTime.Now);
            socio.Estado = "activo";
            
            await _context.SocioExtensionExtensions.AddAsync(socio);
            await _context.SaveChangesAsync();
            
            return Ok(new { 
                mensaje = "Socio registrado correctamente", 
                id = socio.Id 
            });
        }
        
        [HttpPut("socios/{id}")]
        public async Task<IActionResult> PutSocio(int id, SocioExtension socioActualizado)
        {
            var socio = await (from s in _context.SocioExtensionExtensions 
                               where s.Id == id && s.Estado != "inactivo" 
                               select s).FirstOrDefaultAsync();
            
            if (socio == null) 
                return NotFound("No se encontró el socio");
            
            socio.NombreOrganizacion = socioActualizado.NombreOrganizacion;
            socio.TipoOrganizacion = socioActualizado.TipoOrganizacion;
            socio.Contacto = socioActualizado.Contacto;
            socio.Correo = socioActualizado.Correo;
            socio.Telefono = socioActualizado.Telefono;
            socio.Direccion = socioActualizado.Direccion;
            socio.AreaColaboracion = socioActualizado.AreaColaboracion;
            
            await _context.SaveChangesAsync();
            
            return Ok(new { mensaje = "Socio actualizado correctamente" });
        }
        
        [HttpDelete("socios/{id}")]
        public async Task<IActionResult> DeleteSocio(int id)
        {
            var socio = await (from s in _context.SocioExtensionExtensions 
                               where s.Id == id && s.Estado != "inactivo" 
                               select s).FirstOrDefaultAsync();
            
            if (socio == null) 
                return NotFound("No se encontró el socio");
            
            socio.Estado = "inactivo";
            await _context.SaveChangesAsync();
            
            return Ok(new { mensaje = "Socio desactivado correctamente" });
        }
        
        // ========== ESTADÍSTICAS ==========
        
        [HttpGet("estadisticas")]
        public async Task<IActionResult> GetEstadisticas()
        {
            var proyectosActivos = await _context.ProyectoExtensions
                .Where(p => p.Estado == "en_ejecucion" || p.Estado == "planificado")
                .CountAsync();
            
            var actividadesProximas = await _context.ActividadExtensions
                .Where(a => a.Estado == "planificada" && a.FechaActividad >= DateOnly.FromDateTime(DateTime.Now))
                .CountAsync();
            
            var totalVoluntarios = await _context.VoluntarioExtensions
                .Where(v => v.Estado == "activo")
                .CountAsync();
            
            var totalSocios = await _context.SocioExtensionExtensions
                .Where(s => s.Estado == "activo")
                .CountAsync();
            
            return Ok(new
            {
                proyectosActivos,
                actividadesProximas,
                totalVoluntarios,
                totalSocios,
                fechaConsulta = DateTime.Now.ToString("yyyy-MM-dd")
            });
        }
        
        // ========== MÉTODOS PRIVADOS ==========
        
        private async Task<string> GenerarCodigoProyecto()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var year = today.Year;
            
            var lastProyecto = await _context.ProyectoExtensions
                .Where(p => p.FechaCreacion.Year == year)
                .OrderByDescending(p => p.CodigoProyecto)
                .Select(p => p.CodigoProyecto)
                .FirstOrDefaultAsync();
            
            int sequenceNumber = 1;
            
            if (lastProyecto != null)
            {
                var parts = lastProyecto.Split('-');
                if (parts.Length == 3 && int.TryParse(parts[2], out int lastSequence))
                {
                    sequenceNumber = lastSequence + 1;
                }
            }
            
            return $"PRO-{year}-{sequenceNumber:D3}";
        }
        
        private async Task<string> GenerarCodigoActividad()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var year = today.Year;
            var month = today.Month;
            var day = today.Day;
            
            var lastActividad = await _context.ActividadExtensions
                .Where(a => a.FechaCreacion.Year == year && 
                           a.FechaCreacion.Month == month && 
                           a.FechaCreacion.Day == day)
                .OrderByDescending(a => a.CodigoActividad)
                .Select(a => a.CodigoActividad)
                .FirstOrDefaultAsync();
            
            int sequenceNumber = 1;
            
            if (lastActividad != null)
            {
                var parts = lastActividad.Split('-');
                if (parts.Length == 4 && int.TryParse(parts[3], out int lastSequence))
                {
                    sequenceNumber = lastSequence + 1;
                }
            }
            
            return $"ACT-{year}{month:D2}{day:D2}-{sequenceNumber:D3}";
        }
    }
}