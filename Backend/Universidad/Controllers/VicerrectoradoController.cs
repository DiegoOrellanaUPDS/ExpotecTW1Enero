using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entidades;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VicerrectoradoController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        public VicerrectoradoController(AppDbContext context)
        {
            _context = context;
        }
        
        // ========== USUARIOS VICERRECTORADO ==========
        
        [HttpGet("usuarios")]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await (from u in _context.UsuarioVicerrectorados 
                                  where u.Estado != "inactivo" 
                                  select u).ToListAsync();
            return Ok(usuarios);
        }
        
        [HttpGet("usuarios/{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            var usuario = await (from u in _context.UsuarioVicerrectorados 
                                 where u.Id == id && u.Estado != "inactivo" 
                                 select u).FirstOrDefaultAsync();
            
            if (usuario == null) 
                return BadRequest("No se encontró el usuario");
            
            return Ok(usuario);
        }
        
        [HttpPost("usuarios")]
        public async Task<IActionResult> PostUsuario(UsuarioVicerrectorado usuario)
        {
            var existing = await (from u in _context.UsuarioVicerrectorados 
                                  where u.NombreUsuario == usuario.NombreUsuario || 
                                        u.Correo == usuario.Correo 
                                  select u).FirstOrDefaultAsync();
            
            if (existing != null) 
                return BadRequest("Ya existe un usuario con ese nombre o correo");
            
            usuario.CodigoUsuario = Guid.NewGuid().ToString();
            usuario.FechaCreacion = DateOnly.FromDateTime(DateTime.Now);
            usuario.Estado = "activo";
            
            await _context.UsuarioVicerrectorados.AddAsync(usuario);
            await _context.SaveChangesAsync();
            
            return Ok($"Usuario creado correctamente con ID: {usuario.Id}");
        }
        
        [HttpPut("usuarios/{id}")]
        public async Task<IActionResult> PutUsuario(int id, UsuarioVicerrectorado usuarioActualizado)
        {
            var usuario = await (from u in _context.UsuarioVicerrectorados 
                                 where u.Id == id && u.Estado != "inactivo" 
                                 select u).FirstOrDefaultAsync();
            
            if (usuario == null) 
                return BadRequest("No se encontró el usuario");
            
            usuario.NombreCompleto = usuarioActualizado.NombreCompleto;
            usuario.Correo = usuarioActualizado.Correo;
            usuario.Rol = usuarioActualizado.Rol;
            usuario.Departamento = usuarioActualizado.Departamento;
            usuario.Telefono = usuarioActualizado.Telefono;
            usuario.Cargo = usuarioActualizado.Cargo;
            
            await _context.SaveChangesAsync();
            
            return Ok("Usuario actualizado correctamente");
        }
        
        [HttpDelete("usuarios/{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await (from u in _context.UsuarioVicerrectorados 
                                 where u.Id == id && u.Estado != "inactivo" 
                                 select u).FirstOrDefaultAsync();
            
            if (usuario == null) 
                return BadRequest("No se encontró el usuario");
            
            usuario.Estado = "inactivo";
            await _context.SaveChangesAsync();
            
            return Ok("Usuario desactivado correctamente");
        }
        
        // ========== INFORMES VICERRECTORADO ==========
        
        [HttpGet("informes")]
        public async Task<IActionResult> GetInformes()
        {
            var informes = await (from i in _context.InformeVicerrectorados 
                                  where i.Estado != "eliminado" 
                                  orderby i.FechaGeneracion descending 
                                  select i).ToListAsync();
            return Ok(informes);
        }
        
        [HttpGet("informes/{id}")]
        public async Task<IActionResult> GetInforme(int id)
        {
            var informe = await (from i in _context.InformeVicerrectorados 
                                 where i.Id == id && i.Estado != "eliminado" 
                                 select i).FirstOrDefaultAsync();
            
            if (informe == null) 
                return BadRequest("No se encontró el informe");
            
            return Ok(informe);
        }
        
        [HttpPost("informes")]
        public async Task<IActionResult> PostInforme(InformeVicerrectorado informe)
        {
            informe.CodigoInforme = await GenerarCodigoInforme();
            informe.FechaGeneracion = DateOnly.FromDateTime(DateTime.Now);
            informe.Estado = "borrador";
            
            await _context.InformeVicerrectorados.AddAsync(informe);
            await _context.SaveChangesAsync();
            
            return Ok($"Informe creado correctamente con código: {informe.CodigoInforme}");
        }
        
        [HttpPut("informes/{id}/aprobar")]
        public async Task<IActionResult> AprobarInforme(int id)
        {
            var informe = await (from i in _context.InformeVicerrectorados 
                                 where i.Id == id && i.Estado != "eliminado" 
                                 select i).FirstOrDefaultAsync();
            
            if (informe == null) 
                return BadRequest("No se encontró el informe");
            
            informe.Estado = "aprobado";
            informe.FechaAprobacion = DateOnly.FromDateTime(DateTime.Now);
            
            await _context.SaveChangesAsync();
            
            return Ok("Informe aprobado correctamente");
        }
        
        [HttpDelete("informes/{id}")]
        public async Task<IActionResult> DeleteInforme(int id)
        {
            var informe = await (from i in _context.InformeVicerrectorados 
                                 where i.Id == id 
                                 select i).FirstOrDefaultAsync();
            
            if (informe == null) 
                return BadRequest("No se encontró el informe");
            
            informe.Estado = "eliminado";
            await _context.SaveChangesAsync();
            
            return Ok("Informe eliminado correctamente");
        }
        
        // ========== RESOLUCIONES VICERRECTORADO ==========
        
        [HttpGet("resoluciones")]
        public async Task<IActionResult> GetResoluciones()
        {
            var resoluciones = await (from r in _context.ResolucionVicerrectorados 
                                      where r.Estado != "eliminada" 
                                      orderby r.FechaEmision descending 
                                      select r).ToListAsync();
            return Ok(resoluciones);
        }
        
        [HttpGet("resoluciones/{numero}")]
        public async Task<IActionResult> GetResolucion(string numero)
        {
            var resolucion = await (from r in _context.ResolucionVicerrectorados 
                                    where r.NumeroResolucion == numero && r.Estado != "eliminada" 
                                    select r).FirstOrDefaultAsync();
            
            if (resolucion == null) 
                return BadRequest("No se encontró la resolución");
            
            return Ok(resolucion);
        }
        
        [HttpPost("resoluciones")]
        public async Task<IActionResult> PostResolucion(ResolucionVicerrectorado resolucion)
        {
            resolucion.NumeroResolucion = await GenerarNumeroResolucion();
            resolucion.FechaCreacion = DateOnly.FromDateTime(DateTime.Now);
            
            await _context.ResolucionVicerrectorados.AddAsync(resolucion);
            await _context.SaveChangesAsync();
            
            return Ok($"Resolución creada correctamente con número: {resolucion.NumeroResolucion}");
        }
        
        [HttpPut("resoluciones/{id}/derogar")]
        public async Task<IActionResult> DerogarResolucion(int id)
        {
            var resolucion = await (from r in _context.ResolucionVicerrectorados 
                                    where r.Id == id && r.Estado != "eliminada" 
                                    select r).FirstOrDefaultAsync();
            
            if (resolucion == null) 
                return BadRequest("No se encontró la resolución");
            
            resolucion.Estado = "derogada";
            
            await _context.SaveChangesAsync();
            
            return Ok("Resolución derogada correctamente");
        }
        
        [HttpDelete("resoluciones/{id}")]
        public async Task<IActionResult> DeleteResolucion(int id)
        {
            var resolucion = await (from r in _context.ResolucionVicerrectorados 
                                    where r.Id == id 
                                    select r).FirstOrDefaultAsync();
            
            if (resolucion == null) 
                return BadRequest("No se encontró la resolución");
            
            resolucion.Estado = "eliminada";
            await _context.SaveChangesAsync();
            
            return Ok("Resolución eliminada correctamente");
        }
        
        // ========== REUNIONES VICERRECTORADO ==========
        
        [HttpGet("reuniones")]
        public async Task<IActionResult> GetReuniones()
        {
            var reuniones = await (from r in _context.ReunionVicerrectorados 
                                   where r.Estado != "cancelada" 
                                   orderby r.FechaReunion 
                                   select r).ToListAsync();
            return Ok(reuniones);
        }
        
        [HttpGet("reuniones/{id}")]
        public async Task<IActionResult> GetReunion(int id)
        {
            var reunion = await (from r in _context.ReunionVicerrectorados 
                                 where r.Id == id 
                                 select r).FirstOrDefaultAsync();
            
            if (reunion == null) 
                return BadRequest("No se encontró la reunión");
            
            return Ok(reunion);
        }
        
        [HttpPost("reuniones")]
        public async Task<IActionResult> PostReunion(ReunionVicerrectorado reunion)
        {
            reunion.CodigoReunion = await GenerarCodigoReunion();
            reunion.FechaCreacion = DateOnly.FromDateTime(DateTime.Now);
            reunion.Estado = "programada";
            
            await _context.ReunionVicerrectorados.AddAsync(reunion);
            await _context.SaveChangesAsync();
            
            return Ok($"Reunión creada correctamente con código: {reunion.CodigoReunion}");
        }
        
        [HttpPut("reuniones/{id}/realizar")]
        public async Task<IActionResult> RealizarReunion(int id, [FromBody] MinutaRequest request)
        {
            var reunion = await (from r in _context.ReunionVicerrectorados 
                                 where r.Id == id 
                                 select r).FirstOrDefaultAsync();
            
            if (reunion == null) 
                return BadRequest("No se encontró la reunión");
            
            reunion.Estado = "realizada";
            reunion.Acuerdos = request.Acuerdos;
            reunion.MinutaUrl = request.MinutaUrl;
            
            await _context.SaveChangesAsync();
            
            return Ok("Reunión marcada como realizada");
        }
        
        [HttpDelete("reuniones/{id}")]
        public async Task<IActionResult> DeleteReunion(int id)
        {
            var reunion = await (from r in _context.ReunionVicerrectorados 
                                 where r.Id == id 
                                 select r).FirstOrDefaultAsync();
            
            if (reunion == null) 
                return BadRequest("No se encontró la reunión");
            
            reunion.Estado = "cancelada";
            await _context.SaveChangesAsync();
            
            return Ok("Reunión cancelada correctamente");
        }
        
        // ========== MÉTODOS PRIVADOS ==========
        
        private async Task<string> GenerarCodigoInforme()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var year = today.Year;
            var month = today.Month;
            
            var lastInforme = await _context.InformeVicerrectorados
                .Where(i => i.FechaGeneracion.Year == year && 
                           i.FechaGeneracion.Month == month)
                .OrderByDescending(i => i.CodigoInforme)
                .Select(i => i.CodigoInforme)
                .FirstOrDefaultAsync();
            
            int sequenceNumber = 1;
            
            if (lastInforme != null)
            {
                var parts = lastInforme.Split('-');
                if (parts.Length == 3 && int.TryParse(parts[2], out int lastSequence))
                {
                    sequenceNumber = lastSequence + 1;
                }
            }
            
            return $"INF-{year}{month:D2}-{sequenceNumber:D3}";
        }
        
        private async Task<string> GenerarNumeroResolucion()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var year = today.Year;
            
            var lastResolucion = await _context.ResolucionVicerrectorados
                .Where(r => r.FechaEmision.Year == year)
                .OrderByDescending(r => r.NumeroResolucion)
                .Select(r => r.NumeroResolucion)
                .FirstOrDefaultAsync();
            
            int sequenceNumber = 1;
            
            if (lastResolucion != null)
            {
                var parts = lastResolucion.Split('/');
                if (parts.Length == 2 && int.TryParse(parts[0], out int lastSequence))
                {
                    sequenceNumber = lastSequence + 1;
                }
            }
            
            return $"{sequenceNumber:D3}/{year}";
        }
        
        private async Task<string> GenerarCodigoReunion()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var year = today.Year;
            var month = today.Month;
            var day = today.Day;
            
            var lastReunion = await _context.ReunionVicerrectorados
                .Where(r => r.FechaCreacion.Year == year && 
                           r.FechaCreacion.Month == month && 
                           r.FechaCreacion.Day == day)
                .OrderByDescending(r => r.CodigoReunion)
                .Select(r => r.CodigoReunion)
                .FirstOrDefaultAsync();
            
            int sequenceNumber = 1;
            
            if (lastReunion != null)
            {
                var parts = lastReunion.Split('-');
                if (parts.Length == 4 && int.TryParse(parts[3], out int lastSequence))
                {
                    sequenceNumber = lastSequence + 1;
                }
            }
            
            return $"REU-{year}{month:D2}{day:D2}-{sequenceNumber:D3}";
        }
        
        // ========== CLASES AUXILIARES ==========
        
        public class MinutaRequest
        {
            public string? Acuerdos { get; set; }
            public string? MinutaUrl { get; set; }
        }
    }
}