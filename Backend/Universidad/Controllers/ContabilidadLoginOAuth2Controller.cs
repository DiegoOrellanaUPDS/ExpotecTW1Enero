using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Net.Http.Headers;
using Universidad.Entidades;
using Data;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContabilidadLoginOAuth2Controller : ControllerBase
    {
        private const string CLIENT_ID = "1466233124605394974";
        private const string CLIENT_SECRET = "NQVtX6clKvCblBLBZcEEbwdfYlIxnE9I";
        private const string REDIRECT_URI = "http://localhost:5024/api/ContabilidadLoginOAuth2/callback";
        
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _context;

        public ContabilidadLoginOAuth2Controller(
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
                message = "Usa esta URL para autenticarte con Discord"
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

                var tokenData = await tokenResponse.Content.ReadFromJsonAsync<JsonElement>();
                var accessToken = tokenData.GetProperty("access_token").GetString();

                // 2. Obtener información del usuario
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", accessToken);
                
                var userResponse = await _httpClient.GetAsync("https://discord.com/api/users/@me");
                
                if (!userResponse.IsSuccessStatusCode)
                {
                    return StatusCode(500, new { error = "Error al obtener datos del usuario" });
                }

                var userData = await userResponse.Content.ReadFromJsonAsync<JsonElement>();
                var discordId = userData.GetProperty("id").GetString();
                var username = userData.GetProperty("username").GetString();
                var email = userData.GetProperty("email").GetString();
                var discriminator = userData.GetProperty("discriminator").GetString();

                // 3. Generar token único del sistema
                var systemToken = $"ctb_{Guid.NewGuid():N}";
                var nombreCompleto = $"{username}#{discriminator}";

                // 4. Guardar o actualizar en base de datos
                var usuarioExistente = await _context.ContabilidadLoginOauth
                    .FirstOrDefaultAsync(u => u.IdentificadorDiscordUsuarioContabilidad == discordId);

                if (usuarioExistente != null)
                {
                    // Actualizar usuario existente
                    usuarioExistente.TokenSistemaUsuarioContabilidad = systemToken;
                    usuarioExistente.NombreCompletoUsuarioContabilidad = nombreCompleto;
                    usuarioExistente.FechaCreacionUsuarioContabilidad = DateTime.UtcNow;
                }
                else
                {
                    // Crear nuevo usuario
                    var nuevoUsuario = new ContabilidadLoginOauth
                    {
                        IdentificadorDiscordUsuarioContabilidad = discordId ?? "",
                        NombreCompletoUsuarioContabilidad = nombreCompleto,
                        TokenSistemaUsuarioContabilidad = systemToken,
                        FechaCreacionUsuarioContabilidad = DateTime.UtcNow
                    };
                    await _context.ContabilidadLoginOauth.AddAsync(nuevoUsuario);
                }

                await _context.SaveChangesAsync();

                // 5. Retornar respuesta exitosa
                return Ok(new
                {
                    success = true,
                    message = "Autenticación exitosa",
                    token = systemToken,
                    user = new 
                    { 
                        discordId, 
                        username = nombreCompleto, 
                        email 
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
                    details = ex.Message 
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
                    instrucciones = new 
                    {
                        opcion1 = "Enviar en header: Authorization: Bearer {token}",
                        opcion2 = "Enviar en query string: ?token={token}",
                        ejemplo = "/api/ContabilidadLoginOAuth2/verify?token=ctb_abc123"
                    }
                });
            }

            // Buscar usuario por token en la base de datos
            var usuario = await _context.ContabilidadLoginOauth
                .FirstOrDefaultAsync(u => u.TokenSistemaUsuarioContabilidad == systemToken);

            if (usuario != null)
            {
                return Ok(new 
                { 
                    valid = true,
                    message = "Token válido",
                    usuario = new
                    {
                        id = usuario.IdRegistroContabilidadLoginOauth,
                        nombre = usuario.NombreCompletoUsuarioContabilidad,
                        discordId = usuario.IdentificadorDiscordUsuarioContabilidad,
                        fechaCreacion = usuario.FechaCreacionUsuarioContabilidad
                    }
                });
            }

            return Unauthorized(new 
            { 
                valid = false, 
                error = "Token inválido o expirado",
                message = "El token proporcionado no existe o ha expirado"
            });
        }
    }
}