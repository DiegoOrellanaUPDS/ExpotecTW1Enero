using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entidades;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BienestarController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        public BienestarController(AppDbContext context)
        {
            _context = context;
        }
        
        // ========== USUARIOS BIENESTAR ==========
        
        [HttpGet("usuarios")]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await (from u in _context.UsuarioBienestars 
                                  where u.Estado != "inactivo" 
                                  select u).ToListAsync();
            return Ok(usuarios);
        }
        
        [HttpGet("usuarios/{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            var usuario = await (from u in _context.UsuarioBienestars 
                                 where u.Id == id && u.Estado != "inactivo" 
                                 select u).FirstOrDefaultAsync();
            
            if (usuario == null) 
                return BadRequest("No se encontró el usuario");
            
            return Ok(usuario);
        }
        
        [HttpPost("usuarios")]
        public async Task<IActionResult> PostUsuario(UsuarioBienestar usuario)
        {
            var existing = await (from u in _context.UsuarioBienestars 
                                  where u.NombreUsuario == usuario.NombreUsuario || 
                                        u.Correo == usuario.Correo 
                                  select u).FirstOrDefaultAsync();
            
            if (existing != null) 
                return BadRequest("Ya existe un usuario con ese nombre o correo");
            
            usuario.CodigoUsuario = Guid.NewGuid().ToString();
            usuario.FechaCreacion = DateOnly.FromDateTime(DateTime.Now);
            usuario.Estado = "activo";
            
            await _context.UsuarioBienestars.AddAsync(usuario);
            await _context.SaveChangesAsync();
            
            return Ok($"Usuario creado correctamente con ID: {usuario.Id}");
        }
        
        [HttpPut("usuarios/{id}")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioBienestar usuarioActualizado)
        {
            var usuario = await (from u in _context.UsuarioBienestars 
                                 where u.Id == id && u.Estado != "inactivo" 
                                 select u).FirstOrDefaultAsync();
            
            if (usuario == null) 
                return BadRequest("No se encontró el usuario");
            
            usuario.NombreCompleto = usuarioActualizado.NombreCompleto;
            usuario.Correo = usuarioActualizado.Correo;
            usuario.Rol = usuarioActualizado.Rol;
            usuario.Especialidad = usuarioActualizado.Especialidad;
            usuario.Telefono = usuarioActualizado.Telefono;
            usuario.FotoUrl = usuarioActualizado.FotoUrl;
            
            await _context.SaveChangesAsync();
            
            return Ok("Usuario actualizado correctamente");
        }
        
        [HttpDelete("usuarios/{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await (from u in _context.UsuarioBienestars 
                                 where u.Id == id && u.Estado != "inactivo" 
                                 select u).FirstOrDefaultAsync();
            
            if (usuario == null) 
                return BadRequest("No se encontró el usuario");
            
            usuario.Estado = "inactivo";
            await _context.SaveChangesAsync();
            
            return Ok("Usuario desactivado correctamente");
        }
        
        // ========== ESTUDIANTES BENEFICIARIOS ==========
        
        [HttpGet("estudiantes")]
        public async Task<IActionResult> GetEstudiantes()
        {
            var estudiantes = await (from e in _context.EstudianteBeneficiarios 
                                     where e.Estado != "inactivo" 
                                     select e).ToListAsync();
            return Ok(estudiantes);
        }
        
        [HttpGet("estudiantes/{ci}")]
        public async Task<IActionResult> GetEstudiante(string ci)
        {
            var estudiante = await (from e in _context.EstudianteBeneficiarios 
                                    where e.CI == ci && e.Estado != "inactivo" 
                                    select e).FirstOrDefaultAsync();
            
            if (estudiante == null) 
                return BadRequest("No se encontró el estudiante");
            
            return Ok(estudiante);
        }
        
        [HttpPost("estudiantes")]
        public async Task<IActionResult> PostEstudiante(EstudianteBeneficiario estudiante)
        {
            var existing = await (from e in _context.EstudianteBeneficiarios 
                                  where e.CI == estudiante.CI 
                                  select e).FirstOrDefaultAsync();
            
            if (existing != null) 
                return BadRequest("Ya existe un estudiante con esa cédula");
            
            estudiante.FechaRegistro = DateOnly.FromDateTime(DateTime.Now);
            estudiante.Estado = "activo";
            
            await _context.EstudianteBeneficiarios.AddAsync(estudiante);
            await _context.SaveChangesAsync();
            
            return Ok($"Estudiante registrado correctamente con ID: {estudiante.Id}");
        }
        
        [HttpPut("estudiantes/{id}")]
        public async Task<IActionResult> PutEstudiante(int id, EstudianteBeneficiario estudianteActualizado)
        {
            var estudiante = await (from e in _context.EstudianteBeneficiarios 
                                    where e.Id == id && e.Estado != "inactivo" 
                                    select e).FirstOrDefaultAsync();
            
            if (estudiante == null) 
                return BadRequest("No se encontró el estudiante");
            
            estudiante.NombreCompleto = estudianteActualizado.NombreCompleto;
            estudiante.Carrera = estudianteActualizado.Carrera;
            estudiante.Semestre = estudianteActualizado.Semestre;
            estudiante.Correo = estudianteActualizado.Correo;
            estudiante.Telefono = estudianteActualizado.Telefono;
            estudiante.TieneBeca = estudianteActualizado.TieneBeca;
            estudiante.TipoApoyo = estudianteActualizado.TipoApoyo;
            estudiante.Observaciones = estudianteActualizado.Observaciones;
            estudiante.UsuarioBienestarId = estudianteActualizado.UsuarioBienestarId;
            
            await _context.SaveChangesAsync();
            
            return Ok("Estudiante actualizado correctamente");
        }
        
        [HttpDelete("estudiantes/{id}")]
        public async Task<IActionResult> DeleteEstudiante(int id)
        {
            var estudiante = await (from e in _context.EstudianteBeneficiarios 
                                    where e.Id == id && e.Estado != "inactivo" 
                                    select e).FirstOrDefaultAsync();
            
            if (estudiante == null) 
                return BadRequest("No se encontró el estudiante");
            
            estudiante.Estado = "inactivo";
            await _context.SaveChangesAsync();
            
            return Ok("Estudiante desactivado correctamente");
        }
        
        // ========== SOLICITUDES DE APOYO ==========
        
        [HttpGet("solicitudes")]
        public async Task<IActionResult> GetSolicitudes()
        {
            var solicitudes = await (from s in _context.SolicitudApoyos 
                                     where s.Estado != "cancelada" 
                                     orderby s.FechaSolicitud descending 
                                     select s).ToListAsync();
            return Ok(solicitudes);
        }
        
        [HttpGet("solicitudes/{id}")]
        public async Task<IActionResult> GetSolicitud(int id)
        {
            var solicitud = await (from s in _context.SolicitudApoyos 
                                   where s.Id == id && s.Estado != "cancelada" 
                                   select s).FirstOrDefaultAsync();
            
            if (solicitud == null) 
                return BadRequest("No se encontró la solicitud");
            
            return Ok(solicitud);
        }
        
        [HttpPost("solicitudes")]
        public async Task<IActionResult> PostSolicitud(SolicitudApoyo solicitud)
        {
            solicitud.CodigoSolicitud = await GenerarCodigoSolicitud();
            solicitud.FechaSolicitud = DateOnly.FromDateTime(DateTime.Now);
            solicitud.Estado = "pendiente";
            
            await _context.SolicitudApoyos.AddAsync(solicitud);
            await _context.SaveChangesAsync();
            
            return Ok($"Solicitud creada correctamente con código: {solicitud.CodigoSolicitud}");
        }
        
        [HttpPut("solicitudes/{id}/asignar")]
        public async Task<IActionResult> AsignarSolicitud(int id, [FromBody] AsignarRequest request)
        {
            var solicitud = await (from s in _context.SolicitudApoyos 
                                   where s.Id == id && s.Estado != "cancelada" 
                                   select s).FirstOrDefaultAsync();
            
            if (solicitud == null) 
                return BadRequest("No se encontró la solicitud");
            
            solicitud.UsuarioBienestarId = request.UsuarioBienestarId;
            solicitud.Estado = "en_proceso";
            solicitud.FechaAtencion = DateOnly.FromDateTime(DateTime.Now);
            
            await _context.SaveChangesAsync();
            
            return Ok("Solicitud asignada correctamente");
        }
        
        [HttpPut("solicitudes/{id}/completar")]
        public async Task<IActionResult> CompletarSolicitud(int id)
        {
            var solicitud = await (from s in _context.SolicitudApoyos 
                                   where s.Id == id && s.Estado != "cancelada" 
                                   select s).FirstOrDefaultAsync();
            
            if (solicitud == null) 
                return BadRequest("No se encontró la solicitud");
            
            solicitud.Estado = "atendida";
            
            await _context.SaveChangesAsync();
            
            return Ok("Solicitud marcada como completada");
        }
        
        [HttpDelete("solicitudes/{id}")]
        public async Task<IActionResult> DeleteSolicitud(int id)
        {
            var solicitud = await (from s in _context.SolicitudApoyos 
                                   where s.Id == id 
                                   select s).FirstOrDefaultAsync();
            
            if (solicitud == null) 
                return BadRequest("No se encontró la solicitud");
            
            solicitud.Estado = "cancelada";
            await _context.SaveChangesAsync();
            
            return Ok("Solicitud cancelada correctamente");
        }
        
        // ========== ACTIVIDADES BIENESTAR ==========
        
        [HttpGet("actividades")]
        public async Task<IActionResult> GetActividades()
        {
            var actividades = await (from a in _context.ActividadBienestars 
                                     where a.Estado != "cancelada" 
                                     orderby a.FechaActividad 
                                     select a).ToListAsync();
            return Ok(actividades);
        }
        
        [HttpGet("actividades/{id}")]
        public async Task<IActionResult> GetActividad(int id)
        {
            var actividad = await (from a in _context.ActividadBienestars 
                                   where a.Id == id 
                                   select a).FirstOrDefaultAsync();
            
            if (actividad == null) 
                return BadRequest("No se encontró la actividad");
            
            return Ok(actividad);
        }
        
        [HttpPost("actividades")]
        public async Task<IActionResult> PostActividad(ActividadBienestar actividad)
        {
            actividad.CodigoActividad = await GenerarCodigoActividad();
            actividad.FechaCreacion = DateOnly.FromDateTime(DateTime.Now);
            actividad.Estado = "planificada";
            actividad.InscritosActuales = 0;
            
            await _context.ActividadBienestars.AddAsync(actividad);
            await _context.SaveChangesAsync();
            
            return Ok($"Actividad creada correctamente con código: {actividad.CodigoActividad}");
        }
        
        [HttpPut("actividades/{id}")]
        public async Task<IActionResult> PutActividad(int id, ActividadBienestar actividadActualizada)
        {
            var actividad = await (from a in _context.ActividadBienestars 
                                   where a.Id == id 
                                   select a).FirstOrDefaultAsync();
            
            if (actividad == null) 
                return BadRequest("No se encontró la actividad");
            
            actividad.Titulo = actividadActualizada.Titulo;
            actividad.TipoActividad = actividadActualizada.TipoActividad;
            actividad.Descripcion = actividadActualizada.Descripcion;
            actividad.FechaActividad = actividadActualizada.FechaActividad;
            actividad.HoraInicio = actividadActualizada.HoraInicio;
            actividad.HoraFin = actividadActualizada.HoraFin;
            actividad.Lugar = actividadActualizada.Lugar;
            actividad.CapacidadMaxima = actividadActualizada.CapacidadMaxima;
            actividad.MaterialesRequeridos = actividadActualizada.MaterialesRequeridos;
            actividad.CertificadoDisponible = actividadActualizada.CertificadoDisponible;
            actividad.Observaciones = actividadActualizada.Observaciones;
            
            await _context.SaveChangesAsync();
            
            return Ok("Actividad actualizada correctamente");
        }
        
        [HttpDelete("actividades/{id}")]
        public async Task<IActionResult> DeleteActividad(int id)
        {
            var actividad = await (from a in _context.ActividadBienestars 
                                   where a.Id == id 
                                   select a).FirstOrDefaultAsync();
            
            if (actividad == null) 
                return BadRequest("No se encontró la actividad");
            
            actividad.Estado = "cancelada";
            await _context.SaveChangesAsync();
            
            return Ok("Actividad cancelada correctamente");
        }
        
        // ========== MÉTODOS PRIVADOS ==========
        
        private async Task<string> GenerarCodigoSolicitud()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var year = today.Year;
            var month = today.Month;
            var day = today.Day;
            
            var lastSolicitud = await _context.SolicitudApoyos
                .Where(s => s.FechaSolicitud.Year == year && 
                           s.FechaSolicitud.Month == month && 
                           s.FechaSolicitud.Day == day)
                .OrderByDescending(s => s.CodigoSolicitud)
                .Select(s => s.CodigoSolicitud)
                .FirstOrDefaultAsync();
            
            int sequenceNumber = 1;
            
            if (lastSolicitud != null)
            {
                var parts = lastSolicitud.Split('-');
                if (parts.Length == 4 && int.TryParse(parts[3], out int lastSequence))
                {
                    sequenceNumber = lastSequence + 1;
                }
            }
            
            return $"SOL-{year}{month:D2}{day:D2}-{sequenceNumber:D3}";
        }
        
        private async Task<string> GenerarCodigoActividad()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var year = today.Year;
            var month = today.Month;
            
            var lastActividad = await _context.ActividadBienestars
                .Where(a => a.FechaCreacion.Year == year && 
                           a.FechaCreacion.Month == month)
                .OrderByDescending(a => a.CodigoActividad)
                .Select(a => a.CodigoActividad)
                .FirstOrDefaultAsync();
            
            int sequenceNumber = 1;
            
            if (lastActividad != null)
            {
                var parts = lastActividad.Split('-');
                if (parts.Length == 3 && int.TryParse(parts[2], out int lastSequence))
                {
                    sequenceNumber = lastSequence + 1;
                }
            }
            
            return $"ACT-{year}{month:D2}-{sequenceNumber:D3}";
        }
        
        // ========== CLASE AUXILIAR ==========
        
        public class AsignarRequest
        {
            public int UsuarioBienestarId { get; set; }
        }
    }
}