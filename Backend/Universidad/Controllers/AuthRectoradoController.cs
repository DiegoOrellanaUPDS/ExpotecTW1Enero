using Data;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.EntityFrameworkCore; // Necesario para FirstOrDefault async

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/auth/rectorado")] // Ruta específica para tu módulo
    public class AuthRectoradoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _http;

        // 🔴 IMPORTANTE: Pon aquí TUS credenciales de Discord (o las del grupo si comparten)
        private const string CLIENT_ID = "1466416788894908417"; 
        private const string CLIENT_SECRET = "MyxWbC6II8DKI6LdXil1RCDe6xksMuNS";
        
        // 🔴 OJO CON EL PUERTO: Si usas Docker suele ser 5000, verifica dónde corre tu app.
        // Debe ser IGUAL a la que pusiste en el portal de Discord Developers.
        private const string REDIRECT_URI = "http://localhost:5000/api/auth/rectorado/callback";

        public AuthRectoradoController(AppDbContext context)
        {
            _context = context;
            _http = new HttpClient();
        }

        // 1. LOGIN: Redirige al usuario a Discord
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

        // 2. CALLBACK: Discord vuelve aquí con el código
        [HttpGet("callback")]
        public async Task<IActionResult> Callback(string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest("Código de autorización no recibido.");

            // Intercambiar código por Token de Discord
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

            if (!tokenResponse.IsSuccessStatusCode) 
                return Unauthorized("Error al conectar con Discord para obtener token.");

            var tokenJson = JsonDocument.Parse(
                await tokenResponse.Content.ReadAsStringAsync()
            );

            var accessToken = tokenJson.RootElement.GetProperty("access_token").GetString();
            
            // Obtener datos del usuario desde Discord
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var userInfoResponse = await _http.GetAsync("https://discord.com/api/users/@me");
            var userInfo = JsonDocument.Parse(await userInfoResponse.Content.ReadAsStringAsync());

            var username = userInfo.RootElement.GetProperty("username").GetString();
            
            // Discriminator (Discord lo está eliminando, pero por si acaso)
            var discriminator = "0";
            if(userInfo.RootElement.TryGetProperty("discriminator", out var discProp))
                discriminator = discProp.GetString();

            var correo = $"{username}#{discriminator}@discord.com"; // Simulado si no viene email

            // --- LÓGICA DE BASE DE DATOS PROPIA ---
            var usuario = _context.UsuariosRectorado
                .FirstOrDefault(u => u.Nombre == username); // Buscamos por nombre

            if (usuario == null)
            {
                usuario = new UsuarioRectorado
                {
                    Nombre = username,
                    Correo = correo,
                    Rol = "Admin Rectorado",
                    Estado = "Activo"
                };
                _context.UsuariosRectorado.Add(usuario);
            }

            // Generar nuevo token de sesión local
            usuario.TokenSesion = Guid.NewGuid().ToString();
            _context.SaveChanges();

            // Retornar el token al Frontend
            return Ok(new
            {
                mensaje = "Login Rectorado Exitoso",
                token = usuario.TokenSesion,
                usuario = usuario.Nombre,
                rol = usuario.Rol
            });
        }

        // 3. VERIFICAR: Endpoint para comprobar si el token es válido
        [HttpGet("verificar")]
        public IActionResult VerificarSesion([FromQuery] string token)
        {
            var usuario = _context.UsuariosRectorado
                .FirstOrDefault(u => u.TokenSesion == token);

            if (usuario == null || string.IsNullOrEmpty(token))
                return Unauthorized("Sesión inválida");

            return Ok(new { mensaje = "Sesión válida", usuario = usuario.Nombre, rol = usuario.Rol });
        }

        // 4. LOGOUT: Borrar token de la BD
        [HttpPost("logout")]
        public IActionResult Logout([FromHeader] string token) // O [FromQuery] según prefieras
        {
            var usuario = _context.UsuariosRectorado
                .FirstOrDefault(u => u.TokenSesion == token);

            if (usuario == null)
                return Unauthorized("Usuario no encontrado");

            usuario.TokenSesion = "";
            _context.SaveChanges();

            return Ok(new { mensaje = "Sesión cerrada correctamente" });
        }
    }
}