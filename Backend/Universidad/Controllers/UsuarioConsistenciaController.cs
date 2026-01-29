using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entidades;

namespace Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthUsuarioConsistenciaController : ControllerBase
    {
        // private readonly IConfiguration configuration;
        IHttpClientFactory httpClientFactory;
        private readonly AppDbContext context;
        public AuthUsuarioConsistenciaController(IConfiguration configuration,IHttpClientFactory httpClientFactory,AppDbContext context)
        {

            // this.configuration = configuration;
            this.httpClientFactory = httpClientFactory;
            this.context = context;
        }


        [HttpGet("login-consistencia")]
        public async Task<IActionResult>Login()
        {
            var ClientId = "1465001775135195361";
            var redirectUri = "http://localhost:5024/auth/callback";
            
            var url =
                "https://discord.com/oauth2/authorize" +
                "?response_type=code" +
                $"&client_id={ClientId}" +
                $"&redirect_uri={redirectUri}" +
                "&scope=identify";

            return Ok(Redirect(url));
        }

        [HttpGet("callback-consistencia")]
        public async Task<IActionResult> Callback([FromQuery] string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest("No se recibió el código de Discord.");

            var ClientId = "1465001775135195361";
            var ClientSecret = "koN5ftIJmyzcX1PjEZjtn26I1EWBt0hu";
            var redirectUri = "http://localhost:5024/auth/callback"; 

            var client = httpClientFactory.CreateClient();

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
            var tokenData = System.Text.Json.JsonDocument.Parse(tokenJson).RootElement;
            string accessToken = tokenData.GetProperty("access_token").GetString();

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            var userResponse = await client.GetAsync("https://discord.com/api/users/@me");
            if (!userResponse.IsSuccessStatusCode)
                return BadRequest("Error obteniendo datos del usuario de Discord.");

            var userJson = await userResponse.Content.ReadAsStringAsync();
            var userData = System.Text.Json.JsonDocument.Parse(userJson).RootElement;

            string username = userData.GetProperty("username").GetString();

            string tokenSesion = Guid.NewGuid().ToString();
            var usuario = await context.UsuariosConsistencia
                .FirstOrDefaultAsync(u => u.nombreUsuario == username);

            if (usuario == null)
            {
                usuario = new UsuarioConsistencia
                {
                    nombreUsuario = username,
                    rol = "usuario",
                    codigoUsuario = tokenSesion,
                    contrasena= "",
                    estado="activo",
                    fechaDeCreacion = DateOnly.FromDateTime(DateTime.Now)
                };
                context.UsuariosConsistencia.Add(usuario);
            }
            else
            {
                usuario.codigoUsuario = tokenSesion; 
            }

            await context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Usuario autenticado correctamente",
                token = tokenSesion
            });
        }
        [HttpGet("MostrarUsuario")]
        public async Task<ActionResult<IEnumerable<UsuarioConsistencia>>> GetUsuario()
        {
             return await(from ar in context.UsuariosConsistencia
                           where ar.estado == "activo"
                           select ar).ToListAsync();
            
        }
    }

}