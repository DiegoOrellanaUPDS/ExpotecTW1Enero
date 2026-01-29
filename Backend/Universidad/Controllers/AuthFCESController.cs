using Data;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/auth/discord")]
    public class AuthDiscordController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _http;

        // 🔴 Credenciales embebidas (REQUERIDO POR EL DOCENTE)
        private const string CLIENT_ID = "1466345283473641617";
        private const string CLIENT_SECRET = "0_vNwQ3jVz4UaN0sHcFxmbhalzeNEV5b";
        private const string REDIRECT_URI = "http://localhost:5248/api/auth/discord/callback";

        public AuthDiscordController(AppDbContext context)
        {
            _context = context;
            _http = new HttpClient();
        }
        [HttpGet("login")]
        public IActionResult Login()
        {
            var url =
                "https://discord.com/api/oauth2/authorize" +
                $"?client_id={CLIENT_ID}" +
                $"&redirect_uri={Uri.EscapeDataString(REDIRECT_URI)}" +
                $"&response_type=code" +
                $"&scope=identify email";

            return Redirect(url);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback(string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest("Código de autorización no recibido.");
            var tokenRequest = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    { "client_id", CLIENT_ID },
                    { "client_secret", CLIENT_SECRET },
                    { "grant_type", "authorization_code" },
                    { "code", code },
                    { "redirect_uri", REDIRECT_URI }
                }
            );

            var tokenResponse = await _http.PostAsync(
                "https://discord.com/api/oauth2/token",
                tokenRequest
            );

            tokenResponse.EnsureSuccessStatusCode();

            var tokenJson = JsonDocument.Parse(
                await tokenResponse.Content.ReadAsStringAsync()
            );

            var accessToken = tokenJson
                .RootElement
                .GetProperty("access_token")
                .GetString();
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var userInfo = await _http.GetFromJsonAsync<JsonElement>(
                "https://discord.com/api/users/@me"
            );

            var username = userInfo.GetProperty("username").GetString();
            var discriminator = userInfo.GetProperty("discriminator").GetString();

            // Correo simulado (Discord puede no devolver email)
            var correo = $"{username}#{discriminator}@discord.com";
            var usuario = _context.UsuariosFCES
                .FirstOrDefault(u => u.Correo == correo);

            if (usuario == null)
            {
                usuario = new UsuarioFCES
                {
                    Nombre = username,
                    Correo = correo,
                    Rol = "Jefe de Carrera",
                    Estado = "Activo"
                };

                _context.UsuariosFCES.Add(usuario);
            }
            usuario.TokenSesion = Guid.NewGuid().ToString();
            _context.SaveChanges();

            return Ok(new
            {
                token = usuario.TokenSesion,
                usuario.Nombre,
                usuario.Rol
            });
        }
        [HttpGet("verificar")]
        public IActionResult VerificarSesion([FromQuery] string token)
        {
            var usuario = _context.UsuariosFCES
                .FirstOrDefault(u => u.TokenSesion == token);

            if (usuario == null)
                return Unauthorized();

            return Ok(new { mensaje = "Sesión válida", usuario.Nombre });
        }
        [HttpPost("logout")]
        public IActionResult Logout([FromHeader] string token)
        {
            var usuario = _context.UsuariosFCES
                .FirstOrDefault(u => u.TokenSesion == token);

            if (usuario == null)
                return Unauthorized();

            usuario.TokenSesion = "";
            _context.SaveChanges();

            return Ok(new { mensaje = "Sesión cerrada" });
        }
    }
}

