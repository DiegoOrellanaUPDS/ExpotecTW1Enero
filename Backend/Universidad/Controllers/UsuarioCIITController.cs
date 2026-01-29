using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Universidad.Entidades;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("auth")]
    public class UsuarioCIITController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IHttpClientFactory httpClientFactory;

        // Credenciales 
        private readonly string _tenantId = "56de9580-2613-4024-9e29-a0aea78f7ade";
        private readonly string _clientId = "3b3475aa-f6bd-437a-b675-a32a6c76ecd4";
        private readonly string _clientSecret = "~6w8Q~-bW1lTKUp0ZOG9LsruDnWwGa3c0o2-dcP8";
        private readonly string _redirectUri = "http://localhost:5024/auth/callback";

        public UsuarioCIITController(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            this.context = context;
            this.httpClientFactory = httpClientFactory;
        }
        [HttpGet("listar")]
        public async Task<IActionResult> GetUsuarios()
        {
            return Ok(await context.usuarioCIITs.ToListAsync());
        }
        [HttpGet("login")]
        public IActionResult Login()
        {
            var url = $"https://login.microsoftonline.com/{_tenantId}/oauth2/v2.0/authorize?" +
                      $"client_id={_clientId}&response_type=code&redirect_uri={Uri.EscapeDataString(_redirectUri)}&" +
                      $"response_mode=query&scope=User.Read";

            return Ok(Redirect(url)); // CORREGIDO: Redirección directa al navegador
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery] string code)
        {
            if (string.IsNullOrEmpty(code)) return BadRequest("No se recibió código.");

            try
            {
                var client = httpClientFactory.CreateClient();

                // 1. Obtener Token
                var response = await client.PostAsync($"https://login.microsoftonline.com/{_tenantId}/oauth2/v2.0/token",
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["client_id"] = _clientId,
                    ["client_secret"] = _clientSecret,
                    ["code"] = code,
                    ["grant_type"] = "authorization_code",
                    ["redirect_uri"] = _redirectUri
                }));

                var body = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode) return BadRequest(body);

                using var tokenData = JsonDocument.Parse(body);
                var accessToken = tokenData.RootElement.GetProperty("access_token").GetString();

                // 2. Obtener datos del usuario de Microsoft Graph
                var request = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var userResponse = await client.SendAsync(request);
                var userBody = await userResponse.Content.ReadAsStringAsync();

                using var userData = JsonDocument.Parse(userBody);
                var nombreUsuario = userData.RootElement.GetProperty("displayName").GetString();

                // 3. Lógica de DB
                var sessionToken = Guid.NewGuid().ToString();
                var fechaExpiracion = DateTime.UtcNow.AddMinutes(1);

                var usuario = await context.usuarioCIITs.FirstOrDefaultAsync(u => u.nombre == nombreUsuario);

                if (usuario == null)
                {
                    // Crear nuevo
                    usuario = new UsuarioCIIT
                    {
                        nombre = nombreUsuario!,
                        token = sessionToken,
                        expiracion = fechaExpiracion,
                        rol = "CIIT"
                    };
                    await context.usuarioCIITs.AddAsync(usuario);
                }
                else
                {
                    // Actualizar existente
                    usuario.token = sessionToken;
                    usuario.expiracion = fechaExpiracion;
                    // Aseguramos que tenga rol por si acaso
                    if (string.IsNullOrEmpty(usuario.rol)) usuario.rol = "CIIT";
                }

                // Guardar cambios
                await context.SaveChangesAsync();

                return Ok(new
                {
                    mensaje = "Login exitoso",
                    usuario = nombreUsuario,
                    token_sesion = sessionToken,
                    expira_utc = fechaExpiracion
                });
            }
            catch (Exception ex)
            {
                var errorReal = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { error = "Fallo al guardar en DB", detalle = errorReal });
            }
        }

        [HttpGet("verificarSesion")]
        public async Task<IActionResult> VerificarSesion([FromQuery] string token)
        {
            var usuario = await context.usuarioCIITs.FirstOrDefaultAsync(u => u.token == token);

            if (usuario == null || string.IsNullOrEmpty(token))
                return Unauthorized(new { mensaje = "Sesión no válida" });

            if (DateTime.UtcNow > usuario.expiracion)
            {
                usuario.token = "";
                await context.SaveChangesAsync();
                return Unauthorized(new { mensaje = "Sesión expirada" });
            }

            return Ok(new { usuario = usuario.nombre, rol = usuario.rol });
        }
    }
}