using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Data;
using Entidades;

namespace OAuth2DcApi.Controllers
{
    [ApiController]
    [Route("auth")]
    public class UsuarioFIController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public UsuarioFIController(AppDbContext context, IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            var clientId = _config["Discord:ClientId"];
            var redirectUri = _config["Discord:RedirectUri"];
            var scopes = _config["Discord:Scopes"];

            var url = $"https://discord.com/oauth2/authorize?" +
                      $"response_type=code&" +
                      $"client_id={clientId}&" +
                      $"scope={Uri.EscapeDataString(scopes)}&" +
                      $"redirect_uri={Uri.EscapeDataString(redirectUri)}";

            return Ok(Redirect(url));
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery] string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest("No se recibió el código de autorización");

            var clientId = _config["Discord:ClientId"];
            var clientSecret = _config["Discord:ClientSecret"];
            var redirectUri = _config["Discord:RedirectUri"];
            var client = _httpClientFactory.CreateClient();

            var tokenResponse = await client.PostAsync("https://discord.com/api/oauth2/token",
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "client_id", clientId },
                    { "client_secret", clientSecret },
                    { "grant_type", "authorization_code" },
                    { "code", code },
                    { "redirect_uri", redirectUri }
                }));

            var tokenJson = await tokenResponse.Content.ReadAsStringAsync();
            if (!tokenResponse.IsSuccessStatusCode)
                return BadRequest($"Error al obtener token de Discord: {tokenJson}");

            var tokenData = JsonSerializer.Deserialize<Dictionary<string, object>>(tokenJson);
            var accessToken = tokenData["access_token"].ToString();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var userResponse = await client.GetStringAsync("https://discord.com/api/users/@me");
            var userData = JsonSerializer.Deserialize<Dictionary<string, object>>(userResponse);

            string discordId = userData["id"].ToString();
            string username = userData["username"].ToString();
            string email = userData.ContainsKey("email") ? userData["email"].ToString() : "";

            // Busca el usuario en tu entidad UsuarioFI
            var usuario = await _context.UsuarioFIs.FirstOrDefaultAsync(u => u.NombreUsuario == discordId);
            if (usuario == null)
            {
                usuario = new UsuarioFI
                {
                    NombreUsuario = discordId,
                    Token = accessToken,
                    Rol = "User",
                    Estado = "Activo"
                };
                await _context.UsuarioFIs.AddAsync(usuario);
            }
            else
            {
                usuario.Token = accessToken;
            }
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Usuario Discord autenticado y guardado en DB",
                token = accessToken,
                usuario = new { usuario.NombreUsuario, usuario.Rol, usuario.Estado }
            });
        }

        [HttpGet("verificarSesion")]
        public async Task<IActionResult> VerificarSesion([FromQuery] string token)
        {
            if (string.IsNullOrEmpty(token))
                return BadRequest("El token es requerido");

            var usuario = await _context.UsuarioFIs.FirstOrDefaultAsync(u => u.Token == token);
            if (usuario == null)
                return Unauthorized("Sesión inválida o expirada");

            return Ok(new
            {
                mensaje = "Sesión válida",
                usuario = usuario.NombreUsuario,
                rol = usuario.Rol
            });
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout([FromQuery] string token)
        {
            if (string.IsNullOrEmpty(token))
                return BadRequest("El token es requerido");

            var usuario = await _context.UsuarioFIs.FirstOrDefaultAsync(u => u.Token == token);
            if (usuario != null)
            {
                usuario.Token = "";
                await _context.SaveChangesAsync();
            }

            return Ok("Sesión cerrada correctamente y token eliminado de la base de datos");
        }

        [HttpGet("mostrarUsuarios")]
        public async Task<IActionResult> MostrarUsuarios()
        {
            var usuarios = await _context.UsuarioFIs
                .Select(u => new
                {
                    u.NombreUsuario,
                    u.Rol,
                    u.Estado,
                    u.Token
                })
                .ToListAsync();

            return Ok(usuarios);
        }
    }
}
