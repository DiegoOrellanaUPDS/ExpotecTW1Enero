using Data;
using Universidad.Entidades;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using Entidades;
namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthCajaController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public AuthCajaController(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        // 🔹 1. LOGIN → redirige a GitHub
        [HttpGet("discord-login")]
        public IActionResult DiscordLogin()
        {
            var clientId = _config["DiscordOAuth:ClientId"];
            var redirectUri = _config["DiscordOAuth:RedirectUri"];
            var scope = "identify email";

            var discordUrl = $"https://discord.com/api/oauth2/authorize" +
                             $"?client_id={clientId}" +
                             $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                             $"&response_type=code" +
                             $"&scope={scope}";

            return Ok(discordUrl);
        }

        // 🔹 2. CALLBACK → GitHub responde aquí
        [HttpGet("discord-callback")]
        public async Task<IActionResult> DiscordCallback([FromQuery] string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest("Código OAuth no recibido");

            using var client = new HttpClient();

            var dict = new Dictionary<string, string>
    {
        { "client_id", _config["DiscordOAuth:ClientId"] },
        { "client_secret", _config["DiscordOAuth:ClientSecret"] },
        { "grant_type", "authorization_code" },
        { "code", code },
        { "redirect_uri", _config["DiscordOAuth:RedirectUri"] }
    };

            var tokenResponse = await client.PostAsync(
                "https://discord.com/api/oauth2/token",
                new FormUrlEncodedContent(dict)
            );

            if (!tokenResponse.IsSuccessStatusCode)
                return Unauthorized("Error al obtener access token");

            var tokenJson = JsonDocument.Parse(await tokenResponse.Content.ReadAsStringAsync());
            var accessToken = tokenJson.RootElement.GetProperty("access_token").GetString();

            // Obtener info del usuario
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var userJson = await client.GetStringAsync("https://discord.com/api/users/@me");
            var discordUser = JsonDocument.Parse(userJson);

            var username = discordUser.RootElement.GetProperty("username").GetString() +
                           "#" +
                           discordUser.RootElement.GetProperty("discriminator").GetString();

            // Generar token de sesión propio
            var sessionToken = Guid.NewGuid().ToString();

            var usuario = _context.Usuarios_Caja.FirstOrDefault(u => u.Nombre == username);

            if (usuario == null)
            {
                usuario = new Usuario_Caja
                {
                    Nombre = username,
                    Rol = "user",
                    Token = sessionToken,
                    Estado = "Activo"
                };
                _context.Usuarios_Caja.Add(usuario);
            }
            else
            {
                usuario.Token = sessionToken;
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Login exitoso",
                token = sessionToken,
                usuario = username
            });
        }


        // 🔹 3. VERIFICAR SESIÓN
        [HttpGet("verificarSesion/{user}")]
        public IActionResult VerificarSesion(string user)
        {
            if (string.IsNullOrEmpty(user))
                return Unauthorized("User no enviado");

            var usuario = _context.Usuarios_Caja
                .FirstOrDefault(u => u.Nombre == user);

            if (usuario == null)
                return Unauthorized("Sesión inválida");
            if (usuario.Token == "")
                return Unauthorized("No posee token válido.");

            return Ok(new
            {
                mensaje = "Sesión válida",
                usuario = usuario.Nombre
            });
        }

        // 🔹 4. LOGOUT
        [HttpPost("logout/{user}")]
        public IActionResult Logout(string user)
        {
            if (string.IsNullOrEmpty(user))
                return Unauthorized("User no enviado");

            var usuario = _context.Usuarios_Caja
                .FirstOrDefault(u => u.Nombre == user);

            if (usuario == null)
                return Unauthorized("Usuario inexistente");

            usuario.Token = "";
            _context.SaveChanges();

            return Ok("Sesión cerrada correctamente");
        }
    }
}
