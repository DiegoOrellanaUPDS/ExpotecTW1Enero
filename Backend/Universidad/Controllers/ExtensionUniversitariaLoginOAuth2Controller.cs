using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Net.Http.Headers;
using Data;
using Entidades;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExtensionUniversitariaLoginOAuth2Controller : ControllerBase
    {
        private const string CLIENT_ID = "1465246687399252010";
        private const string CLIENT_SECRET = "2yer8WGf38Y63fJaFt0NxMDwFnpvey2y";
        private const string REDIRECT_URI = "http://localhost:5024/api/ExtensionUniversitariaLoginOAuth2/callback";
        
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _context;

        public ExtensionUniversitariaLoginOAuth2Controller(
            IHttpClientFactory httpClientFactory,
            AppDbContext context)
        {
            _httpClient = httpClientFactory.CreateClient();
            _context = context;
        }

        // ========== ENDPOINT 1: OBTENER URL DE DISCORD ==========
        [HttpGet("auth-url")]
        public IActionResult GetAuthUrl()
        {
            return Ok(new 
            { 
                url = $"https://discord.com/api/oauth2/authorize?client_id={CLIENT_ID}&redirect_uri={Uri.EscapeDataString(REDIRECT_URI)}&response_type=code&scope=identify%20email",
                message = "Usa esta URL para autenticarte con Discord en Extensión Universitaria",
                modulo = "Extensión Universitaria"
            });
        }

        // ========== ENDPOINT 2: CALLBACK DE DISCORD ==========
        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery] string code)
        {
            if (string.IsNullOrEmpty(code)) 
            {
                return BadRequest(new { error = "Código de autorización requerido" });
            }

            try
            {
                // 1. Intercambiar código por token de acceso
                var tokenResponse = await _httpClient.PostAsync(
                    "https://discord.com/api/oauth2/token",
                    new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        ["client_id"] = CLIENT_ID,
                        ["client_secret"] = CLIENT_SECRET,
                        ["grant_type"] = "authorization_code",
                        ["code"] = code,
                        ["redirect_uri"] = REDIRECT_URI
                    })
                );

                if (!tokenResponse.IsSuccessStatusCode)
                {
                    return StatusCode(500, new { error = "Error al comunicarse con Discord" });
                }

                var tokenJson = await tokenResponse.Content.ReadAsStringAsync();
                var tokenData = JsonSerializer.Deserialize<JsonElement>(tokenJson);
                
                if (!tokenData.TryGetProperty("access_token", out var accessTokenProp))
                {
                    return StatusCode(500, new { error = "No se recibió access_token de Discord" });
                }
                var accessToken = accessTokenProp.GetString();

                // 2. Obtener información del usuario
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", accessToken);
                
                var userResponse = await _httpClient.GetAsync("https://discord.com/api/users/@me");
                
                if (!userResponse.IsSuccessStatusCode)
                {
                    return StatusCode(500, new { error = "Error al obtener datos del usuario" });
                }

                var userJson = await userResponse.Content.ReadAsStringAsync();
                var userData = JsonSerializer.Deserialize<JsonElement>(userJson);

                // Extraer datos de usuario con validación
                string discordId = userData.TryGetProperty("id", out var idProp) ? idProp.GetString() ?? "" : "";
                string username = userData.TryGetProperty("username", out var usernameProp) ? usernameProp.GetString() ?? "" : "";
                string discriminator = userData.TryGetProperty("discriminator", out var discProp) ? discProp.GetString() ?? "" : "";
                
                // Email puede ser opcional, usar valor por defecto si no existe
                string email = userData.TryGetProperty("email", out var emailProp) ? 
                    emailProp.GetString() ?? $"{discordId}@discord.user" : 
                    $"{discordId}@discord.user";
                
                // Global name puede ser null
                string globalName = userData.TryGetProperty("global_name", out var globalProp) ? 
                    globalProp.GetString() ?? username : 
                    username;

                // 3. Generar token único del sistema para Extensión Universitaria
                var systemToken = $"extuniv_{Guid.NewGuid():N}";
                var nombreCompleto = !string.IsNullOrEmpty(globalName) ? globalName : $"{username}#{discriminator}";

                // 4. Guardar o actualizar en base de datos
                var usuarioExistente = await _context.UsuariosExtuniv
                    .FirstOrDefaultAsync(u => u.IdentificadorDiscordUsuarioExtuniv == discordId);

                if (usuarioExistente != null)
                {
                    // Actualizar usuario existente
                    usuarioExistente.TokenSistemaUsuarioExtuniv = systemToken;
                    usuarioExistente.NombreCompletoUsuarioExtuniv = nombreCompleto;
                    usuarioExistente.CorreoUsuarioExtuniv = email;
                    usuarioExistente.UltimoAccesoUsuarioExtuniv = DateTime.UtcNow;
                    usuarioExistente.EstadoUsuarioExtuniv = "activo";
                }
                else
                {
                    // Crear nuevo usuario en Extensión Universitaria
                    var nuevoUsuario = new Usuario_extuniv
                    {
                        IdentificadorDiscordUsuarioExtuniv = discordId,
                        NombreCompletoUsuarioExtuniv = nombreCompleto,
                        CorreoUsuarioExtuniv = email,
                        TokenSistemaUsuarioExtuniv = systemToken,
                        RolUsuarioExtuniv = "usuario",
                        EstadoUsuarioExtuniv = "activo",
                        FechaCreacionUsuarioExtuniv = DateTime.UtcNow,
                        UltimoAccesoUsuarioExtuniv = DateTime.UtcNow
                    };
                    await _context.UsuariosExtuniv.AddAsync(nuevoUsuario);
                }

                await _context.SaveChangesAsync();

                // 5. Retornar respuesta exitosa
                return Ok(new
                {
                    success = true,
                    message = "Autenticación exitosa en Extensión Universitaria",
                    token = systemToken,
                    user = new 
                    { 
                        discordId, 
                        nombreCompleto, 
                        email,
                        username = $"{username}#{discriminator}",
                        modulo = "Extensión Universitaria"
                    },
                    expires_in = 3600, // 1 hora en segundos
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                { 
                    error = "Error interno del servidor",
                    details = ex.Message,
                    modulo = "Extensión Universitaria"
                });
            }
        }

        // ========== ENDPOINT 3: VERIFICAR TOKEN ==========
        [HttpGet("verify")]
        public async Task<IActionResult> Verify(
            [FromHeader(Name = "Authorization")] string authHeader = null,
            [FromQuery] string token = null)
        {
            // Tomar token del header o del query string
            var systemToken = authHeader?.Replace("Bearer ", "") ?? token;
            
            if (string.IsNullOrEmpty(systemToken))
            {
                return BadRequest(new 
                { 
                    error = "Token de autenticación requerido",
                    modulo = "Extensión Universitaria",
                    instrucciones = new 
                    {
                        opcion1 = "Enviar en header: Authorization: Bearer {token}",
                        opcion2 = "Enviar en query string: ?token={token}",
                        ejemplo = "/api/ExtensionUniversitariaLoginOAuth2/verify?token=extuniv_abc123"
                    }
                });
            }

            // Buscar usuario por token en la base de datos
            var usuario = await _context.UsuariosExtuniv
                .FirstOrDefaultAsync(u => u.TokenSistemaUsuarioExtuniv == systemToken);

            if (usuario != null && usuario.EstadoUsuarioExtuniv == "activo")
            {
                return Ok(new 
                { 
                    valid = true,
                    message = "Token válido para Extensión Universitaria",
                    usuario = new
                    {
                        id = usuario.IdRegistroUsuarioExtuniv,
                        nombre = usuario.NombreCompletoUsuarioExtuniv,
                        correo = usuario.CorreoUsuarioExtuniv,
                        rol = usuario.RolUsuarioExtuniv,
                        discordId = usuario.IdentificadorDiscordUsuarioExtuniv,
                        fechaCreacion = usuario.FechaCreacionUsuarioExtuniv,
                        ultimoAcceso = usuario.UltimoAccesoUsuarioExtuniv,
                        modulo = "Extensión Universitaria"
                    }
                });
            }

            return Unauthorized(new 
            { 
                valid = false, 
                error = "Token inválido o expirado",
                message = "El token proporcionado no existe, ha expirado o el usuario está inactivo",
                modulo = "Extensión Universitaria"
            });
        }

        // ========== ENDPOINT 4: CERRAR SESIÓN ==========
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(
            [FromHeader(Name = "Authorization")] string authHeader = null)
        {
            var systemToken = authHeader?.Replace("Bearer ", "");
            
            if (string.IsNullOrEmpty(systemToken))
            {
                return BadRequest(new 
                { 
                    error = "Token de autenticación requerido",
                    modulo = "Extensión Universitaria"
                });
            }

            var usuario = await _context.UsuariosExtuniv
                .FirstOrDefaultAsync(u => u.TokenSistemaUsuarioExtuniv == systemToken);

            if (usuario != null)
            {
                // Invalidar token
                usuario.TokenSistemaUsuarioExtuniv = "";
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Sesión cerrada exitosamente",
                    modulo = "Extensión Universitaria"
                });
            }

            return Ok(new
            {
                success = true,
                message = "Sesión ya estaba cerrada",
                modulo = "Extensión Universitaria"
            });
        }

        // ========== ENDPOINT 5: OBTENER PERFIL ==========
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile(
            [FromHeader(Name = "Authorization")] string authHeader = null)
        {
            var systemToken = authHeader?.Replace("Bearer ", "");
            
            if (string.IsNullOrEmpty(systemToken))
            {
                return Unauthorized(new 
                { 
                    error = "Token de autenticación requerido",
                    modulo = "Extensión Universitaria"
                });
            }

            var usuario = await _context.UsuariosExtuniv
                .FirstOrDefaultAsync(u => u.TokenSistemaUsuarioExtuniv == systemToken && u.EstadoUsuarioExtuniv == "activo");

            if (usuario == null)
            {
                return Unauthorized(new 
                { 
                    error = "Token inválido o usuario inactivo",
                    modulo = "Extensión Universitaria"
                });
            }

            return Ok(new
            {
                success = true,
                usuario = new
                {
                    id = usuario.IdRegistroUsuarioExtuniv,
                    nombreCompleto = usuario.NombreCompletoUsuarioExtuniv,
                    correo = usuario.CorreoUsuarioExtuniv,
                    rol = usuario.RolUsuarioExtuniv,
                    estado = usuario.EstadoUsuarioExtuniv,
                    fechaCreacion = usuario.FechaCreacionUsuarioExtuniv,
                    ultimoAcceso = usuario.UltimoAccesoUsuarioExtuniv,
                    modulo = "Extensión Universitaria"
                }
            });
        }

        // ========== ENDPOINT 6: LISTAR USUARIOS (solo admin) ==========
        [HttpGet("usuarios")]
        public async Task<IActionResult> GetUsuarios(
            [FromHeader(Name = "Authorization")] string authHeader = null)
        {
            var systemToken = authHeader?.Replace("Bearer ", "");
            
            if (string.IsNullOrEmpty(systemToken))
            {
                return Unauthorized(new { error = "Token de autenticación requerido" });
            }

            var usuario = await _context.UsuariosExtuniv
                .FirstOrDefaultAsync(u => u.TokenSistemaUsuarioExtuniv == systemToken);

            if (usuario == null || usuario.RolUsuarioExtuniv != "administrador")
            {
                return Forbid("Solo administradores pueden ver la lista de usuarios");
            }

            var usuarios = await _context.UsuariosExtuniv
                .Where(u => u.EstadoUsuarioExtuniv == "activo")
                .Select(u => new
                {
                    u.IdRegistroUsuarioExtuniv,
                    u.NombreCompletoUsuarioExtuniv,
                    u.CorreoUsuarioExtuniv,
                    u.RolUsuarioExtuniv,
                    u.FechaCreacionUsuarioExtuniv
                })
                .ToListAsync();

            return Ok(new
            {
                success = true,
                count = usuarios.Count,
                usuarios
            });
        }
    }
}