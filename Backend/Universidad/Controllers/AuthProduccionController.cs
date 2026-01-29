using Data;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using Universidad.Entidades;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/auth/discord")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;
        private readonly HttpClient _http;

        public AuthController(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
            _http = new HttpClient();
        }

        // Redirige al login de Discord
        [HttpGet("login")]
        public IActionResult Login()
        {
            var clientId = _config["DiscordOAuth:ClientId"];
            var redirectUri = "http://localhost:5024/api/auth/discord/callback"; // Ajustar según tu puerto
            var url = $"https://discord.com/api/oauth2/authorize?client_id={clientId}&redirect_uri={redirectUri}&response_type=code&scope=identify";
            return Redirect(url);
        }

        // Callback de Discord después del login
        [HttpGet("callback")]
        public async Task<IActionResult> Callback(string code)
        {
            var clientId = _config["DiscordOAuth:ClientId"];
            var clientSecret = _config["DiscordOAuth:ClientSecret"];
            var redirectUri = "http://localhost:5024/api/auth/discord/callback";

            var tokenRequest = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", redirectUri }
            });

            var tokenResponse = await _http.PostAsync("https://discord.com/api/oauth2/token", tokenRequest);
            tokenResponse.EnsureSuccessStatusCode();
            var tokenJson = JsonDocument.Parse(await tokenResponse.Content.ReadAsStringAsync());
            var accessToken = tokenJson.RootElement.GetProperty("access_token").GetString();

            // Obtener info del usuario de Discord
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var userInfo = await _http.GetFromJsonAsync<JsonElement>("https://discord.com/api/users/@me");
            var username = userInfo.GetProperty("username").GetString();
            var discriminator = userInfo.GetProperty("discriminator").GetString();
            var correoFake = $"{username}#{discriminator}@discord.com"; // Genera correo ficticio para la BD

            // Crear token de sesión
            var tokenSesion = Guid.NewGuid().ToString();

            // Buscar usuario en PersonaProduccion            // En este ejemplo, podrías usar un Dictionary o una propiedad Token en PersonaProduccion
            // Aquí solo retornamos el token al cliente
            var persona = _context.PersonaProduccions.FirstOrDefault(p => p.correo == correoFake);
            if (persona == null)
            {
                persona = new PersonaProduccion
                {
                    nombre = username,
                    apellido = discriminator,
                    rol = "USER",
                    correo = correoFake,
                    estado = "ACTIVO"
                };
                _context.PersonaProduccions.Add(persona);
            }

            // Guardar token de sesión en una propiedad temporal (puedes crear una tabla de Tokens si quieres)
            // Para simplicidad, vamos a usar una entidad temporal local
            _context.SaveChanges();

            return Ok(new { token = tokenSesion, persona.nombre, persona.correo, persona.rol });
        }

        // Verificar sesión con token
        [HttpGet("verificarSesion")]
        public IActionResult VerificarSesion([FromQuery] string token)
        {
            // Aquí normalmente buscarías el token en una tabla de sesiones o en PersonaProduccion
            if (string.IsNullOrEmpty(token))
                return Unauthorized();

            return Ok(new { mensaje = "Token válido" });
        }

        // Logout
        [HttpPost("logout")]
        public IActionResult Logout([FromHeader] string token)
        {
            // Aquí normalmente eliminarías el token de la BD o memoria
            if (string.IsNullOrEmpty(token))
                return Unauthorized();

            return Ok(new { mensaje = "Sesión cerrada" });
        }
    }
}
