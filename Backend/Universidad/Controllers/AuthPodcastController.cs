using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entidades;
using System.Text.Json;

namespace Controllers
{
    [ApiController]
    [Route("api/podcast/auth")]
    public class AuthPodcastController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppDbContext _context;
        
        public AuthPodcastController(IHttpClientFactory httpClientFactory, AppDbContext context)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
        }
        
        [HttpGet("login")]
        public IActionResult Login()
        {
            var ClientId = "TU_CLIENT_ID_PODCAST";
            var redirectUri = "http://localhost:5024/api/podcast/auth/callback";
            
            var url = "https://discord.com/oauth2/authorize" +
                     "?response_type=code" +
                     $"&client_id={ClientId}" +
                     $"&redirect_uri={redirectUri}" +
                     "&scope=identify";
            
            return Ok(new { redirectUrl = url });
        }
        
        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery] string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest("No se recibió el código de Discord.");
            
            var ClientId = "TU_CLIENT_ID_PODCAST";
            var ClientSecret = "TU_CLIENT_SECRET_PODCAST";
            var redirectUri = "http://localhost:5024/api/podcast/auth/callback";
            
            var client = _httpClientFactory.CreateClient();
            
            var tokenResponse = await client.PostAsync(
                "https://discord.com/api/oauth2/token",
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["client_id"] = ClientId,
                    ["client_secret"] = ClientSecret,
                    ["grant_type"] = "authorization_code",
                    ["code"] = code,
                    ["redirect_uri"] = redirectUri
                })
            );
            
            if (!tokenResponse.IsSuccessStatusCode)
                return BadRequest("Error obteniendo token de Discord.");
            
            var tokenJson = await tokenResponse.Content.ReadAsStringAsync();
            var tokenData = JsonDocument.Parse(tokenJson).RootElement;
            string accessToken = tokenData.GetProperty("access_token").GetString();
            
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            
            var userResponse = await client.GetAsync("https://discord.com/api/users/@me");
            if (!userResponse.IsSuccessStatusCode)
                return BadRequest("Error obteniendo datos del usuario de Discord.");
            
            var userJson = await userResponse.Content.ReadAsStringAsync();
            var userData = JsonDocument.Parse(userJson).RootElement;
            
            string username = userData.GetProperty("username").GetString();
            
            string tokenSesion = Guid.NewGuid().ToString();
            
            var usuario = await (from u in _context.UsuarioPodcasts
                                 where u.NombreUsuario == username
                                 select u).FirstOrDefaultAsync();
            
            if (usuario == null)
            {
                usuario = new UsuarioPodcast
                {
                    NombreUsuario = username,
                    NombreCompleto = $"{username} (Discord)",
                    Correo = $"{username}@discord.app",
                    Rol = "productor",
                    CodigoUsuario = tokenSesion,
                    Estado = "activo",
                    FechaCreacion = DateOnly.FromDateTime(DateTime.Now)
                };
                await _context.UsuarioPodcasts.AddAsync(usuario);
            }
            else
            {
                usuario.CodigoUsuario = tokenSesion;
            }
            
            await _context.SaveChangesAsync();
            
            // Guardar en sesión
            HttpContext.Session.SetString("PodcastToken", tokenSesion);
            HttpContext.Session.SetString("PodcastUserId", usuario.Id.ToString());
            
            return Ok(new
            {
                mensaje = "Usuario autenticado correctamente",
                token = tokenSesion,
                usuario = new
                {
                    id = usuario.Id,
                    nombre = usuario.NombreCompleto,
                    rol = usuario.Rol,
                    correo = usuario.Correo,
                    especialidad = usuario.Especialidad
                }
            });
        }
        
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            try
            {
                // Limpiar sesión
                HttpContext.Session.Clear();
                
                return Ok(new
                {
                    mensaje = "Sesión cerrada correctamente",
                    logoutExitoso = true,
                    fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al cerrar sesión",
                    error = ex.Message,
                    logoutExitoso = false
                });
            }
        }
    }
}