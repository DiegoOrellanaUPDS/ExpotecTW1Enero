using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text.Json;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Universidad.Entidades;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/asistente/auth")]
    public class AuthUsuarioAsistenteController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        
        public AuthUsuarioAsistenteController(
            IConfiguration config,
            AppDbContext context,
            IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        // GET: api/asistente/auth/login
        [HttpGet("login")]
        public IActionResult Login()
        {
            var clientId = _config["DiscordOAuth:ClientId"] ?? "1466437284453945440";
            var redirectUri = "http://localhost:5024/api/asistentes/auth/callback";
            
            var url = $"http://discord.com/api/oauth2/authorize" +
                $"?client_id={clientId}" +
                $"&redirect_uri={redirectUri}" +
                $"&response_type=code" +
                $"&scope=identify%20email";
            
            return Redirect(url);
        }

        // GET: api/asistente/auth/callback
        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery] string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                    return BadRequest(new { mensaje = "No se recibió el código de Discord." });

                var clientId = _config["DiscordOAuth:ClientId"] ?? "1466437284453945440";
                var clientSecret = _config["DiscordOAuth:ClientSecret"] ?? "CXysEba37boJ48swS3yMBS3-SoT1agc6";
                var redirectUri = "http://localhost:5024/api/asistentes/auth/callback";

                var client = _httpClientFactory.CreateClient();

                // 1. Obtener token de Discord (estilo del primer código)
                var tokenRequest = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "client_id", clientId },
                    { "client_secret", clientSecret },
                    { "grant_type", "authorization_code" },
                    { "code", code },
                    { "redirect_uri", redirectUri }
                });

                var tokenResponse = await client.PostAsync("http://discord.com/api/oauth2/token", tokenRequest);
                
                if (!tokenResponse.IsSuccessStatusCode)
                {
                    var errorContent = await tokenResponse.Content.ReadAsStringAsync();
                    return BadRequest(new { 
                        mensaje = "Error obteniendo token de Discord",
                        error = errorContent 
                    });
                }

                var tokenJson = JsonDocument.Parse(await tokenResponse.Content.ReadAsStringAsync());
                var accessToken = tokenJson.RootElement.GetProperty("access_token").GetString();

                // 2. Obtener datos del usuario de Discord
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var userInfo = await client.GetFromJsonAsync<JsonElement>("http://discord.com/api/users/@me");
                
                string discordId = userInfo.GetProperty("id").GetString();
                string username = userInfo.GetProperty("username").GetString();
                string discriminator = userInfo.GetProperty("discriminator").GetString();
                string email = userInfo.TryGetProperty("email", out var emailProp) 
                    ? emailProp.GetString() 
                    : $"{username}@discord.user";

                // 3. Buscar o crear usuario en nuestra base de datos
                string tokenSesion = Guid.NewGuid().ToString();
                var usuario = await _context.UsuariosAsistente
                    .FirstOrDefaultAsync(u => u.Email == email || u.Cedula == discordId);

                bool esNuevoUsuario = false;

                if (usuario == null)
                {
                    esNuevoUsuario = true;
                    usuario = new UsuarioAsistente
                    {
                        Cedula = discordId,
                        Nombre = username,
                        Apellido = discriminator,
                        Email = email,
                        Rol = "Estudiante",
                        Telefono = "",
                        Activo = true,
                        FechaCreacion = DateTime.Now
                    };
                    _context.UsuariosAsistente.Add(usuario);
                }
                else
                {
                    usuario.Activo = true;
                    usuario.FechaCreacion = DateTime.Now;
                }

                await _context.SaveChangesAsync();

                // 4. Retornar respuesta
                return Ok(new
                {
                    mensaje = esNuevoUsuario ? "Usuario registrado exitosamente" : "Usuario autenticado exitosamente",
                    token = tokenSesion,
                    usuario = new
                    {
                        id = usuario.Id,
                        cedula = usuario.Cedula,
                        nombre = usuario.Nombre,
                        apellido = usuario.Apellido,
                        email = usuario.Email,
                        telefono = usuario.Telefono,
                        rol = usuario.Rol,
                        activo = usuario.Activo,
                        fechaCreacion = usuario.FechaCreacion,
                        discordId = discordId,
                        discordUsername = $"{username}#{discriminator}"
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    mensaje = "Error interno del servidor",
                    error = ex.Message 
                });
            }
        }

        // GET: api/asistente/auth/login-link
        [HttpGet("login-link")]
        public IActionResult GetLoginLink()
        {
            try
            {
                var clientId = _config["DiscordOAuth:ClientId"] ?? "1466437284453945440";
                var redirectUri = "http://localhost:5024/api/asistentes/auth/callback";
                
                var url = $"http://discord.com/api/oauth2/authorize" +
                    $"?client_id={clientId}" +
                    $"&redirect_uri={redirectUri}" +
                    $"&response_type=code" +
                    $"&scope=identify%20email";
                
                return Ok(new { 
                    mensaje = "Link de autenticación con Discord",
                    discordAuthUrl = url,
                    instrucciones = "Usa esta URL para iniciar sesión con Discord"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    mensaje = "Error al generar link de Discord",
                    error = ex.Message 
                });
            }
        }

        // GET: api/asistente/auth/verificar-sesion
        [HttpGet("verificar-sesion")]
        public IActionResult VerificarSesion([FromQuery] string token)
        {
            // Aquí normalmente buscarías el token en una tabla de sesiones
            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { mensaje = "Token requerido" });

            return Ok(new { 
                mensaje = "Token válido",
                valido = true
            });
        }

        // POST: api/asistente/auth/logout
        [HttpPost("logout")]
        public IActionResult Logout([FromHeader] string token)
        {
            // Aquí normalmente eliminarías el token de la BD o memoria
            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { mensaje = "Token requerido" });

            return Ok(new { mensaje = "Sesión cerrada exitosamente" });
        }

        // ==================== CRUD ADICIONAL ====================

        // GET: api/asistente/auth/usuarios-activos
        [HttpGet("usuarios-activos")]
        public async Task<IActionResult> GetUsuariosActivos()
        {
            try
            {
                var usuarios = await _context.UsuariosAsistente
                    .Where(u => u.Activo)
                    .OrderBy(u => u.Nombre)
                    .ThenBy(u => u.Apellido)
                    .Select(u => new
                    {
                        u.Id,
                        u.Cedula,
                        u.Nombre,
                        u.Apellido,
                        u.Email,
                        u.Telefono,
                        u.Rol,
                        u.Activo,
                        u.FechaCreacion
                    })
                    .ToListAsync();

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    mensaje = "Error al obtener usuarios activos",
                    error = ex.Message 
                });
            }
        }

        // GET: api/asistente/auth/usuarios
        [HttpGet("usuarios")]
        public async Task<IActionResult> GetAllUsuarios()
        {
            try
            {
                var usuarios = await _context.UsuariosAsistente
                    .OrderBy(u => u.Nombre)
                    .ThenBy(u => u.Apellido)
                    .Select(u => new
                    {
                        u.Id,
                        u.Cedula,
                        u.Nombre,
                        u.Apellido,
                        u.Email,
                        u.Telefono,
                        u.Rol,
                        u.Activo,
                        u.FechaCreacion
                    })
                    .ToListAsync();

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    mensaje = "Error al obtener usuarios",
                    error = ex.Message 
                });
            }
        }

        // PUT: api/asistente/auth/desactivar/{id}
        [HttpPut("desactivar/{id}")]
        public async Task<IActionResult> DesactivarUsuario(int id)
        {
            try
            {
                var usuario = await _context.UsuariosAsistente.FindAsync(id);
                
                if (usuario == null)
                    return NotFound(new { mensaje = "Usuario no encontrado" });
                
                usuario.Activo = false;
                await _context.SaveChangesAsync();
                
                return Ok(new { 
                    mensaje = "Usuario desactivado exitosamente",
                    id = usuario.Id,
                    nombre = $"{usuario.Nombre} {usuario.Apellido}"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    mensaje = "Error al desactivar usuario",
                    error = ex.Message 
                });
            }
        }

        // PUT: api/asistente/auth/activar/{id}
        [HttpPut("activar/{id}")]
        public async Task<IActionResult> ActivarUsuario(int id)
        {
            try
            {
                var usuario = await _context.UsuariosAsistente.FindAsync(id);
                
                if (usuario == null)
                    return NotFound(new { mensaje = "Usuario no encontrado" });
                
                usuario.Activo = true;
                await _context.SaveChangesAsync();
                
                return Ok(new { 
                    mensaje = "Usuario activado exitosamente",
                    id = usuario.Id,
                    nombre = $"{usuario.Nombre} {usuario.Apellido}"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    mensaje = "Error al activar usuario",
                    error = ex.Message 
                });
            }
        }

        // GET: api/asistente/auth/usuario/{id}
        [HttpGet("usuario/{id}")]
        public async Task<IActionResult> GetUsuarioPorId(int id)
        {
            try
            {
                var usuario = await _context.UsuariosAsistente.FindAsync(id);
                
                if (usuario == null)
                    return NotFound(new { mensaje = "Usuario no encontrado" });
                
                return Ok(new
                {
                    usuario.Id,
                    usuario.Cedula,
                    usuario.Nombre,
                    usuario.Apellido,
                    usuario.Email,
                    usuario.Telefono,
                    usuario.Rol,
                    usuario.Activo,
                    usuario.FechaCreacion
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    mensaje = "Error al obtener usuario",
                    error = ex.Message 
                });
            }
        }

        // POST: api/asistente/auth/crear-usuario
        [HttpPost("crear-usuario")]
        public async Task<IActionResult> CrearUsuario([FromBody] UsuarioAsistente usuario)
        {
            try
            {
                if (usuario == null)
                    return BadRequest(new { mensaje = "Datos de usuario inválidos" });

                // Validar campos requeridos
                if (string.IsNullOrWhiteSpace(usuario.Cedula))
                    return BadRequest(new { mensaje = "La cédula es requerida" });
                
                if (string.IsNullOrWhiteSpace(usuario.Nombre))
                    return BadRequest(new { mensaje = "El nombre es requerido" });
                
                if (string.IsNullOrWhiteSpace(usuario.Email))
                    return BadRequest(new { mensaje = "El email es requerido" });

                // Validar cédula única
                if (await _context.UsuariosAsistente.AnyAsync(u => u.Cedula == usuario.Cedula))
                    return BadRequest(new { mensaje = "Ya existe un usuario con esta cédula" });

                // Validar email único
                if (await _context.UsuariosAsistente.AnyAsync(u => u.Email == usuario.Email))
                    return BadRequest(new { mensaje = "Ya existe un usuario con este email" });

                // Asignar valores por defecto
                usuario.Activo = true;
                usuario.FechaCreacion = DateTime.Now;
                
                // Si no se especifica rol, usar "Estudiante" por defecto
                if (string.IsNullOrWhiteSpace(usuario.Rol))
                    usuario.Rol = "Estudiante";

                _context.UsuariosAsistente.Add(usuario);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUsuarioPorId), new { id = usuario.Id }, new
                {
                    mensaje = "Usuario creado exitosamente",
                    data = usuario
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    mensaje = "Error al crear usuario",
                    error = ex.Message 
                });
            }
        }
    }
}