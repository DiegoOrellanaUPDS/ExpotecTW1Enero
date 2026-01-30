using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entidades;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PodcastController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        public PodcastController(AppDbContext context)
        {
            _context = context;
        }
        
        // ========== USUARIOS PODCAST ==========
        
        [HttpGet("usuarios")]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await (from u in _context.UsuarioPodcasts 
                                  where u.Estado != "inactivo" 
                                  select u).ToListAsync();
            return Ok(usuarios);
        }
        
        [HttpGet("usuarios/{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            var usuario = await (from u in _context.UsuarioPodcasts 
                                 where u.Id == id && u.Estado != "inactivo" 
                                 select u).FirstOrDefaultAsync();
            
            if (usuario == null) 
                return BadRequest("No se encontró el usuario");
            
            return Ok(usuario);
        }
        
        [HttpPost("usuarios")]
        public async Task<IActionResult> PostUsuario(UsuarioPodcast usuario)
        {
            var existing = await (from u in _context.UsuarioPodcasts 
                                  where u.NombreUsuario == usuario.NombreUsuario || 
                                        u.Correo == usuario.Correo 
                                  select u).FirstOrDefaultAsync();
            
            if (existing != null) 
                return BadRequest("Ya existe un usuario con ese nombre o correo");
            
            usuario.CodigoUsuario = Guid.NewGuid().ToString();
            usuario.FechaCreacion = DateOnly.FromDateTime(DateTime.Now);
            usuario.Estado = "activo";
            
            await _context.UsuarioPodcasts.AddAsync(usuario);
            await _context.SaveChangesAsync();
            
            return Ok($"Usuario creado correctamente con ID: {usuario.Id}");
        }
        
        [HttpPut("usuarios/{id}")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioPodcast usuarioActualizado)
        {
            var usuario = await (from u in _context.UsuarioPodcasts 
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
            usuario.Biografia = usuarioActualizado.Biografia;
            
            await _context.SaveChangesAsync();
            
            return Ok("Usuario actualizado correctamente");
        }
        
        [HttpDelete("usuarios/{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await (from u in _context.UsuarioPodcasts 
                                 where u.Id == id && u.Estado != "inactivo" 
                                 select u).FirstOrDefaultAsync();
            
            if (usuario == null) 
                return BadRequest("No se encontró el usuario");
            
            usuario.Estado = "inactivo";
            await _context.SaveChangesAsync();
            
            return Ok("Usuario desactivado correctamente");
        }
        
        // ========== EPISODIOS PODCAST ==========
        
        [HttpGet("episodios")]
        public async Task<IActionResult> GetEpisodios([FromQuery] string? estado = null)
        {
            var query = from e in _context.EpisodioPodcasts 
                        where e.Estado != "eliminado" 
                        select e;
            
            if (!string.IsNullOrEmpty(estado))
                query = query.Where(e => e.Estado == estado);
            
            var episodios = await query
                .OrderByDescending(e => e.FechaCreacion)
                .ToListAsync();
            
            return Ok(episodios);
        }
        
        [HttpGet("episodios/{codigo}")]
        public async Task<IActionResult> GetEpisodio(string codigo)
        {
            var episodio = await (from e in _context.EpisodioPodcasts 
                                  where e.CodigoEpisodio == codigo && e.Estado != "eliminado" 
                                  select e).FirstOrDefaultAsync();
            
            if (episodio == null) 
                return BadRequest("No se encontró el episodio");
            
            return Ok(episodio);
        }
        
        [HttpPost("episodios")]
        public async Task<IActionResult> PostEpisodio(EpisodioPodcast episodio)
        {
            episodio.CodigoEpisodio = await GenerarCodigoEpisodio();
            episodio.FechaCreacion = DateOnly.FromDateTime(DateTime.Now);
            episodio.Estado = "planificado";
            episodio.Reproducciones = 0;
            
            await _context.EpisodioPodcasts.AddAsync(episodio);
            await _context.SaveChangesAsync();
            
            return Ok($"Episodio creado correctamente con código: {episodio.CodigoEpisodio}");
        }
        
        [HttpPut("episodios/{id}/publicar")]
        public async Task<IActionResult> PublicarEpisodio(int id)
        {
            var episodio = await (from e in _context.EpisodioPodcasts 
                                  where e.Id == id && e.Estado != "eliminado" 
                                  select e).FirstOrDefaultAsync();
            
            if (episodio == null) 
                return BadRequest("No se encontró el episodio");
            
            episodio.Estado = "publicado";
            episodio.FechaPublicacion = DateOnly.FromDateTime(DateTime.Now);
            
            await _context.SaveChangesAsync();
            
            return Ok("Episodio publicado correctamente");
        }
        
        [HttpDelete("episodios/{id}")]
        public async Task<IActionResult> DeleteEpisodio(int id)
        {
            var episodio = await (from e in _context.EpisodioPodcasts 
                                  where e.Id == id 
                                  select e).FirstOrDefaultAsync();
            
            if (episodio == null) 
                return BadRequest("No se encontró el episodio");
            
            episodio.Estado = "eliminado";
            await _context.SaveChangesAsync();
            
            return Ok("Episodio eliminado correctamente");
        }
        
        // ========== PROGRAMACIÓN PODCAST ==========
        
        [HttpGet("programacion")]
        public async Task<IActionResult> GetProgramacion()
        {
            var programacion = await (from p in _context.ProgramacionPodcasts 
                                      where p.Estado != "cancelado" 
                                      orderby p.FechaInicio 
                                      select p).ToListAsync();
            return Ok(programacion);
        }
        
        [HttpGet("programacion/{codigo}")]
        public async Task<IActionResult> GetPrograma(string codigo)
        {
            var programa = await (from p in _context.ProgramacionPodcasts 
                                  where p.CodigoProgramacion == codigo && p.Estado != "cancelado" 
                                  select p).FirstOrDefaultAsync();
            
            if (programa == null) 
                return BadRequest("No se encontró el programa");
            
            return Ok(programa);
        }
        
        [HttpPost("programacion")]
        public async Task<IActionResult> PostProgramacion(ProgramacionPodcast programacion)
        {
            programacion.CodigoProgramacion = await GenerarCodigoProgramacion();
            programacion.FechaCreacion = DateOnly.FromDateTime(DateTime.Now);
            programacion.Estado = "activo";
            programacion.TotalEpisodios = 0;
            programacion.EpisodiosPublicados = 0;
            
            await _context.ProgramacionPodcasts.AddAsync(programacion);
            await _context.SaveChangesAsync();
            
            return Ok($"Programa creado correctamente con código: {programacion.CodigoProgramacion}");
        }
        
        [HttpPut("programacion/{id}/finalizar")]
        public async Task<IActionResult> FinalizarPrograma(int id)
        {
            var programa = await (from p in _context.ProgramacionPodcasts 
                                  where p.Id == id && p.Estado != "cancelado" 
                                  select p).FirstOrDefaultAsync();
            
            if (programa == null) 
                return BadRequest("No se encontró el programa");
            
            programa.Estado = "finalizado";
            programa.FechaFin = DateOnly.FromDateTime(DateTime.Now);
            
            await _context.SaveChangesAsync();
            
            return Ok("Programa finalizado correctamente");
        }
        
        [HttpDelete("programacion/{id}")]
        public async Task<IActionResult> DeleteProgramacion(int id)
        {
            var programa = await (from p in _context.ProgramacionPodcasts 
                                  where p.Id == id 
                                  select p).FirstOrDefaultAsync();
            
            if (programa == null) 
                return BadRequest("No se encontró el programa");
            
            programa.Estado = "cancelado";
            await _context.SaveChangesAsync();
            
            return Ok("Programa cancelado correctamente");
        }
        
        // ========== RECURSOS PODCAST ==========
        
        [HttpGet("recursos")]
        public async Task<IActionResult> GetRecursos([FromQuery] string? estado = null)
        {
            var query = from r in _context.RecursoPodcasts 
                        where r.Estado != "eliminado" 
                        select r;
            
            if (!string.IsNullOrEmpty(estado))
                query = query.Where(r => r.Estado == estado);
            
            var recursos = await query
                .OrderByDescending(r => r.FechaRegistro)
                .ToListAsync();
            
            return Ok(recursos);
        }
        
        [HttpGet("recursos/{codigo}")]
        public async Task<IActionResult> GetRecurso(string codigo)
        {
            var recurso = await (from r in _context.RecursoPodcasts 
                                 where r.CodigoRecurso == codigo && r.Estado != "eliminado" 
                                 select r).FirstOrDefaultAsync();
            
            if (recurso == null) 
                return BadRequest("No se encontró el recurso");
            
            return Ok(recurso);
        }
        
        [HttpPost("recursos")]
        public async Task<IActionResult> PostRecurso(RecursoPodcast recurso)
        {
            recurso.CodigoRecurso = await GenerarCodigoRecurso();
            recurso.FechaRegistro = DateOnly.FromDateTime(DateTime.Now);
            recurso.Estado = "disponible";
            
            await _context.RecursoPodcasts.AddAsync(recurso);
            await _context.SaveChangesAsync();
            
            return Ok($"Recurso registrado correctamente con código: {recurso.CodigoRecurso}");
        }
        
        [HttpPut("recursos/{id}/estado")]
        public async Task<IActionResult> CambiarEstadoRecurso(int id, [FromBody] CambiarEstadoRequest request)
        {
            var recurso = await (from r in _context.RecursoPodcasts 
                                 where r.Id == id && r.Estado != "eliminado" 
                                 select r).FirstOrDefaultAsync();
            
            if (recurso == null) 
                return BadRequest("No se encontró el recurso");
            
            recurso.Estado = request.NuevoEstado;
            
            await _context.SaveChangesAsync();
            
            return Ok($"Estado del recurso cambiado a: {request.NuevoEstado}");
        }
        
        [HttpDelete("recursos/{id}")]
        public async Task<IActionResult> DeleteRecurso(int id)
        {
            var recurso = await (from r in _context.RecursoPodcasts 
                                 where r.Id == id 
                                 select r).FirstOrDefaultAsync();
            
            if (recurso == null) 
                return BadRequest("No se encontró el recurso");
            
            recurso.Estado = "eliminado";
            await _context.SaveChangesAsync();
            
            return Ok("Recurso eliminado correctamente");
        }
        
        // ========== ESTADÍSTICAS PODCAST ==========
        
        [HttpGet("estadisticas")]
        public async Task<IActionResult> GetEstadisticas()
        {
            var totalEpisodios = await _context.EpisodioPodcasts
                .Where(e => e.Estado == "publicado")
                .CountAsync();
            
            var totalReproducciones = await _context.EpisodioPodcasts
                .Where(e => e.Estado == "publicado")
                .SumAsync(e => e.Reproducciones);
            
            var programasActivos = await _context.ProgramacionPodcasts
                .Where(p => p.Estado == "activo")
                .CountAsync();
            
            var recursosDisponibles = await _context.RecursoPodcasts
                .Where(r => r.Estado == "disponible")
                .CountAsync();
            
            return Ok(new
            {
                totalEpisodios,
                totalReproducciones,
                programasActivos,
                recursosDisponibles,
                fechaConsulta = DateTime.Now.ToString("yyyy-MM-dd")
            });
        }
        
        // ========== MÉTODOS PRIVADOS ==========
        
        private async Task<string> GenerarCodigoEpisodio()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var year = today.Year;
            var month = today.Month;
            
            var lastEpisodio = await _context.EpisodioPodcasts
                .Where(e => e.FechaCreacion.Year == year && 
                           e.FechaCreacion.Month == month)
                .OrderByDescending(e => e.CodigoEpisodio)
                .Select(e => e.CodigoEpisodio)
                .FirstOrDefaultAsync();
            
            int sequenceNumber = 1;
            
            if (lastEpisodio != null)
            {
                var parts = lastEpisodio.Split('-');
                if (parts.Length == 3 && int.TryParse(parts[2], out int lastSequence))
                {
                    sequenceNumber = lastSequence + 1;
                }
            }
            
            return $"EPS-{year}{month:D2}-{sequenceNumber:D3}";
        }
        
        private async Task<string> GenerarCodigoProgramacion()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var year = today.Year;
            
            var lastPrograma = await _context.ProgramacionPodcasts
                .Where(p => p.FechaCreacion.Year == year)
                .OrderByDescending(p => p.CodigoProgramacion)
                .Select(p => p.CodigoProgramacion)
                .FirstOrDefaultAsync();
            
            int sequenceNumber = 1;
            
            if (lastPrograma != null)
            {
                var parts = lastPrograma.Split('-');
                if (parts.Length == 3 && int.TryParse(parts[2], out int lastSequence))
                {
                    sequenceNumber = lastSequence + 1;
                }
            }
            
            return $"PRO-{year}-{sequenceNumber:D3}";
        }
        
        private async Task<string> GenerarCodigoRecurso()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var year = today.Year;
            var month = today.Month;
            
            var lastRecurso = await _context.RecursoPodcasts
                .Where(r => r.FechaRegistro.Year == year && 
                           r.FechaRegistro.Month == month)
                .OrderByDescending(r => r.CodigoRecurso)
                .Select(r => r.CodigoRecurso)
                .FirstOrDefaultAsync();
            
            int sequenceNumber = 1;
            
            if (lastRecurso != null)
            {
                var parts = lastRecurso.Split('-');
                if (parts.Length == 3 && int.TryParse(parts[2], out int lastSequence))
                {
                    sequenceNumber = lastSequence + 1;
                }
            }
            
            return $"REC-{year}{month:D2}-{sequenceNumber:D3}";
        }
        
        // ========== CLASE AUXILIAR ==========
        
        public class CambiarEstadoRequest
        {
            public string NuevoEstado { get; set; }
        }
    }
}