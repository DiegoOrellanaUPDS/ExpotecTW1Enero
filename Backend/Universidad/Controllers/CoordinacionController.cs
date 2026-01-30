using Microsoft.AspNetCore.Mvc;
using Data;
using Entidades;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json; // Corrige el error de 'JsonConvert'
using System.Net.Http.Headers; // Corrige el error de 'AuthenticationHeaderValue'
using Microsoft.AspNetCore.Mvc; // Para los atributos [HttpGet] y [FromQuery]


namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoordinacionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CoordinacionController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Coordinacion (Muestra todas las licencias)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Licencia>>> GetLicencias()
        {
            return await _context.Licencias.ToListAsync();
        }

        // GET: api/Coordinacion/{id} (Busca una específica)
        [HttpGet("{id}")]
        public async Task<ActionResult<Licencia>> GetLicencia(int id)
        {
            var licencia = await _context.Licencias.FindAsync(id);
            if (licencia == null) return NotFound();
            return licencia;
        }

        // POST: api/Coordinacion/registrar-licencia
        [HttpPost("registrar-licencia")]
        public async Task<IActionResult> RegistrarLicencia([FromBody] Licencia licencia)
        {
            if (licencia == null) return BadRequest();
            _context.Licencias.Add(licencia);
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Licencia guardada en DB", id = licencia.Id });
        }

        // PUT: api/Coordinacion/{id} (Para aprobar o editar)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLicencia(int id, [FromBody] Licencia licencia)
        {
            if (id != licencia.Id) return BadRequest();
            _context.Entry(licencia).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Coordinacion/{id} (Para borrar)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLicencia(int id)
        {
            var licencia = await _context.Licencias.FindAsync(id);
            if (licencia == null) return NotFound();
            _context.Licencias.Remove(licencia);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private readonly string ClientId = "1466437585600778506";
        private readonly string ClientSecret = "TUL2tQWeP8GBKb7-GsvLcbDp_pdRSyOz3";
        private readonly string RedirectUri = "http://localhost:5024/api/Coordinacion/callback";

        [HttpGet("login-discord")]
        public IActionResult LoginDiscord()
        {
            // Genera la URL de autorización que viste en la documentación de Discord
            var url = $"https://discord.com/api/oauth2/authorize" +
                    $"?client_id={ClientId}" +
                    $"&redirect_uri={Uri.EscapeDataString(RedirectUri)}" +
                    $"&response_type=code" +
                    $"&scope=identify%20email";

            return Redirect(url); // Redirige automáticamente al usuario a Discord
        }

        // 3. MÉTODO CALLBACK: Recibe el código y gestiona la sesión
        [HttpGet("callback")]
        public async Task<IActionResult> DiscordCallback([FromQuery] string code)
        {
            if (string.IsNullOrEmpty(code)) 
                return BadRequest("No se proporcionó el código de autorización.");

            using var client = new HttpClient();

            // PASO A: Intercambiar el código por un Access Token
            var tokenParams = new Dictionary<string, string>
            {
                { "client_id", ClientId },
                { "client_secret", ClientSecret },
                { "grant_type", "authorization_code" },
                { "code", code },
                { "redirect_uri", RedirectUri }
            };

            var tokenResponse = await client.PostAsync("https://discord.com/api/oauth2/token", new FormUrlEncodedContent(tokenParams));
            var tokenContent = await tokenResponse.Content.ReadAsStringAsync();

            if (!tokenResponse.IsSuccessStatusCode)
                return BadRequest($"Error al obtener token: {tokenContent}");

            var tokenData = JsonConvert.DeserializeObject<dynamic>(tokenContent);
            string accessToken = tokenData.access_token;

            // PASO B: Usar el token para obtener el perfil del usuario (@me)
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var userResponse = await client.GetAsync("https://discord.com/api/users/@me");
            var userContent = await userResponse.Content.ReadAsStringAsync();
            var userData = JsonConvert.DeserializeObject<dynamic>(userContent);

            // PASO C: Respuesta final (Gestión de la sesión)
            return Ok(new
            {
                mensaje = "Autenticación exitosa con Discord",
                token_acceso = accessToken,
                expira_en = tokenData.expires_in,
                usuario = new {
                    id = userData.id,
                    nombre = userData.username,
                    email = userData.email,
                    avatar = $"https://cdn.discordapp.com/avatars/{userData.id}/{userData.avatar}.png"
                }
            });
        }
    }
}