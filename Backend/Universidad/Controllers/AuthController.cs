using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Data;
using Entidades;

namespace OAuthGoogle.Controllers
{
    [ApiController]
    [Route("SolicitudLoguin")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppDbContext _db;

        public AuthController(IConfiguration config, IHttpClientFactory httpClientFactory, AppDbContext db)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
            _db = db;
        }

        // GET: SolicitudLoguin/login
        [HttpGet("login")]
        public IActionResult Login()
        {
            var clientId = _config["Google:ClientId"];
            var redirectUri = "http://localhost:5024/SolicitudLoguin/callbackgoogle"; // nuevo redirectUri

            var url = $"https://accounts.google.com/o/oauth2/v2/auth" +
                      $"?client_id={clientId}" +
                      $"&redirect_uri={redirectUri}" +
                      $"&response_type=code" +
                      $"&scope=openid%20email%20profile";

            return Ok(new { loginUrl = url });
        }

        // GET: SolicitudLoguin/callbackgoogle?code=xxxx
        [HttpGet("callbackgoogle")]
        public async Task<IActionResult> Callback([FromQuery] string code)
        {
            var clientId = _config["Google:ClientId"];
            var clientSecret = _config["Google:ClientSecret"];
            var redirectUri = "http://localhost:5024/SolicitudLoguin/callbackgoogle"; // nuevo redirectUri

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsync("https://oauth2.googleapis.com/token",
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["code"] = code,
                    ["client_id"] = clientId!,
                    ["client_secret"] = clientSecret!,
                    ["redirect_uri"] = redirectUri,
                    ["grant_type"] = "authorization_code"
                }));

            var body = await response.Content.ReadAsStringAsync();
            HttpContext.Session.SetString("GoogleToken", body);

            using var doc = JsonDocument.Parse(body);
            var accessToken = doc.RootElement.GetProperty("access_token").GetString();

            var userInfoResponse = await client.GetAsync("https://www.googleapis.com/oauth2/v3/userinfo?access_token=" + accessToken);
            var userInfo = await userInfoResponse.Content.ReadAsStringAsync();
            var userJson = JsonDocument.Parse(userInfo).RootElement;

            // Guardar en la base de datos
            var login = new GoogleLogin
            {
                GoogleId = userJson.GetProperty("sub").GetString(),
                Email = userJson.GetProperty("email").GetString(),
                Name = userJson.GetProperty("name").GetString(),
                Picture = userJson.GetProperty("picture").GetString(),
                AccessToken = accessToken,
                LoginDate = DateTime.UtcNow
            };

            _db.GoogleLogins.Add(login);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Autenticación exitosa",
                userInfo = login
            });
        }

        // GET: SolicitudLoguin/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GoogleLogin>> GetById(int id)
        {
            var login = await _db.GoogleLogins.FindAsync(id);
            if (login == null)
                return NotFound(new { message = "Registro no encontrado" });

            return Ok(login);
        }

        // GET: SolicitudLoguin/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<GoogleLogin>>> GetAll()
        {
            var logins = await Task.FromResult(_db.GoogleLogins.ToList());
            return Ok(logins);
        }
    }
}
