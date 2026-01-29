using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Universidad.Entidades;
using Data;
using System.Text.Json;
using System.Net.Http.Headers;

namespace Controllers
{
    [ApiController]
    [Route("api/google-auth")]
    public class AuthController : ControllerBase
    {
        private const string ClientId = "197924293278-o23ebj29rpuup59cke562p8206kp305s.apps.googleusercontent.com";
        private const string ClientSecret = "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";
        private const string RedirectUri = "http://localhost:5024/api/google-auth/callback";

        private readonly AppDbContext context;
        private readonly IHttpClientFactory httpClientFactory;

        public AuthController(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            this.context = context;
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            var url = "https://accounts.google.com/o/oauth2/v2/auth" +
                      $"?client_id={ClientId}" +
                      $"&redirect_uri={RedirectUri}" +
                      "&response_type=code" +
                      "&scope=openid%20profile%20email" +
                      "&access_type=offline";

            return Redirect(url);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery] string code)
        {
            if (string.IsNullOrEmpty(code)) return BadRequest("No se recibió el código de Google");

            var client = httpClientFactory.CreateClient();

            var tokenResponse = await client.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = ClientId,
                ["client_secret"] = ClientSecret,
                ["code"] = code,
                ["grant_type"] = "authorization_code",
                ["redirect_uri"] = RedirectUri
            }));

            var tokenBody = await tokenResponse.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(tokenBody);
            var accessToken = jsonDoc.RootElement.GetProperty("access_token").GetString();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var userResponse = await client.GetAsync("https://www.googleapis.com/oauth2/v3/userinfo");
            var userBody = await userResponse.Content.ReadAsStringAsync();
            using var userJson = JsonDocument.Parse(userBody);
            
            var googleId = userJson.RootElement.GetProperty("sub").GetString();
            var name = userJson.RootElement.GetProperty("name").GetString();

            var usuario = await context.UsuariosPracticasProfesionales
                .FirstOrDefaultAsync(u => u.Username == name);

            if (usuario == null)
            {
                usuario = new UsuarioPracticasProfesionales
                {
                    codigo = "USR-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                    Username = name,
                    Token = accessToken,
                    Estado = true
                };
                context.UsuariosPracticasProfesionales.Add(usuario);
            }
            else
            {
                usuario.Token = accessToken;
            }

            await context.SaveChangesAsync();

            HttpContext.Session.SetString("GoogleToken", accessToken);
            HttpContext.Session.SetString("Usuario", name);

            return Ok(new { mensaje = "Autenticado correctamente", usuario = usuario.Username, codigo = usuario.codigo });
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = HttpContext.Session.GetString("GoogleToken");
            var username = HttpContext.Session.GetString("Usuario");

            if (!string.IsNullOrEmpty(token))
            {
                try 
                {
                    var client = httpClientFactory.CreateClient();
                    var revokeUrl = $"https://oauth2.googleapis.com/revoke?token={token}";
                    
                    await client.PostAsync(revokeUrl, null);
                }
                catch (Exception) {}
            }

            // 3. LIMPIAR LA BASE DE DATOS LOCAL
            if (!string.IsNullOrEmpty(username))
            {
                var usuario = await context.UsuariosPracticasProfesionales
                    .FirstOrDefaultAsync(u => u.Username == username);

                if (usuario != null)
                {
                    usuario.Token = string.Empty;
                    usuario.Estado = false;
                    await context.SaveChangesAsync();
                }
            }
            HttpContext.Session.Clear();

            return Ok(new { 
                mensaje = "Sesión cerrada correctamente. Token anulado y base de datos actualizada." 
            });
        }
    }
}