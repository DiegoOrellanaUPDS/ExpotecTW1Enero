using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Data;
using Universidad.Entidades;
using System.Text.Json;

namespace universidad.Controllers
{
    [ApiController]
    [Route("auth/biblioteca")]
    public class OAuthBibliotecaController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppDbContext _context;

        public OAuthBibliotecaController(IConfiguration configuration,
                                         IHttpClientFactory httpClientFactory,
                                         AppDbContext context)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _context = context;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            var clientId = _configuration["Discord:ClientId"];
            var redirectUri = "http://localhost:5024/auth/biblioteca/callback";

            var url = $"https://discord.com/api/oauth2/authorize" +
                      $"?client_id={clientId}" +
                      $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                      $"&response_type=code&scope=identify email";

            return Redirect(url);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery] string code)
        {
            var client = _httpClientFactory.CreateClient();

            var tokenRequest = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = _configuration["Discord:ClientId"],
                ["client_secret"] = _configuration["Discord:ClientSecret"],
                ["grant_type"] = "authorization_code",
                ["code"] = code,
                ["redirect_uri"] = "http://localhost:5024/auth/biblioteca/callback"
            });

            var tokenResponse = await client.PostAsync("https://discord.com/api/oauth2/token", tokenRequest);
            var tokenJson = await tokenResponse.Content.ReadAsStringAsync();
            var tokenData = JsonSerializer.Deserialize<DiscordTokenResponse>(tokenJson);

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenData.access_token}");

            var userJson = await client.GetStringAsync("https://discord.com/api/users/@me");
            var discordUser = JsonSerializer.Deserialize<DiscordUser>(userJson);

            var usuario = await _context.BibliotecaUsuariosOAuth
                .FirstOrDefaultAsync(u => u.DiscordId == discordUser.id);

            if (usuario == null)
            {
                usuario = new BibliotecaUsuarioOAuth
                {
                    Nombre = discordUser.username,
                    DiscordId = discordUser.id,
                    Token = tokenData.access_token
                };
                _context.BibliotecaUsuariosOAuth.Add(usuario);
            }
            else
            {
                usuario.Token = tokenData.access_token;
                _context.BibliotecaUsuariosOAuth.Update(usuario);
            }

            await _context.SaveChangesAsync();

            HttpContext.Session.SetString("DiscordToken", tokenData.access_token);
            HttpContext.Session.SetInt32("UserId", usuario.Id);

            return Ok(new { mensaje = "Login Biblioteca OK", usuario });
        }
    }

    public class DiscordTokenResponse
    {
        public string access_token { get; set; }
    }

    public class DiscordUser
    {
        public string id { get; set; }
        public string username { get; set; }
    }
}
