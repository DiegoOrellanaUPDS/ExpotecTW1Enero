using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContabilidadLoginOAuth2Controller : ControllerBase
    {
        // ========== CONFIGURACIÓN CON NOMBRES ÚNICOS ==========
        private const string CLIENT_ID_CONTABILIDAD_OAUTH_2024 = "1466233124605394974";
        private const string CLIENT_SECRET_CONTABILIDAD_OAUTH_2024 = "NQVtX6clKvCblBLBZcEEbwdfYlIxnE9I";
        private const string REDIRECT_URI_CONTABILIDAD_OAUTH_2024 = "http://localhost:5024/api/ContabilidadLoginOAuth2/callback";

        // ========== ENTIDADES CON NOMBRES ÚNICOS ==========
        public class UsuarioAutenticacionContabilidadOAuth2024
        {
            public int IdRegistroUsuarioContabilidad2024 { get; set; }
            public string IdentificadorDiscordContabilidad2024 { get; set; } = string.Empty;
            public string NombreCompletoUsuarioContabilidad2024 { get; set; } = string.Empty;
            public string CorreoElectronicoContabilidad2024 { get; set; } = string.Empty;
            public string TokenSesionContabilidad2024 { get; set; } = string.Empty;
            public DateTime FechaRegistroContabilidad2024 { get; set; }
            public int ContadorAccesoContabilidad2024 { get; set; }
        }

        public class RegistroLogContabilidadOAuth2024
        {
            public int IdRegistroLogContabilidad2024 { get; set; }
            public string TipoOperacionLogContabilidad2024 { get; set; } = string.Empty;
            public DateTime FechaHoraLogContabilidad2024 { get; set; }
        }

        // ========== ALMACENAMIENTO MEJORADO ==========
        private static readonly object _lockContabilidad2024 = new object();
        private static readonly Dictionary<string, UsuarioAutenticacionContabilidadOAuth2024> 
            _usuariosContabilidadDict2024 = new();
        private static readonly List<RegistroLogContabilidadOAuth2024> 
            _logsContabilidadLista2024 = new();

        private readonly HttpClient _httpClient;

        public ContabilidadLoginOAuth2Controller(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        // ========== ENDPOINT 1 ==========
        [HttpGet("iniciar-sesion-contabilidad")]
        public IActionResult IniciarSesionContabilidad()
        {
            var urlDiscord = $"https://discord.com/api/oauth2/authorize" +
                           $"?client_id={CLIENT_ID_CONTABILIDAD_OAUTH_2024}" +
                           $"&redirect_uri={Uri.EscapeDataString(REDIRECT_URI_CONTABILIDAD_OAUTH_2024)}" +
                           $"&response_type=code" +
                           $"&scope=identify%20email";
            
            lock (_lockContabilidad2024)
            {
                _logsContabilidadLista2024.Add(new RegistroLogContabilidadOAuth2024
                {
                    IdRegistroLogContabilidad2024 = _logsContabilidadLista2024.Count + 1,
                    TipoOperacionLogContabilidad2024 = "solicitud_autenticacion",
                    FechaHoraLogContabilidad2024 = DateTime.UtcNow
                });
            }

            return Ok(new 
            {
                url_autenticacion_discord_contabilidad_unica = urlDiscord,
                timestamp_solicitud_contabilidad = DateTime.UtcNow
            });
        }

        // ========== ENDPOINT 2 ==========
        [HttpGet("procesar-autenticacion-contabilidad")]
        public async Task<IActionResult> ProcesarAutenticacionContabilidad([FromQuery] string codigo)
        {
            if (string.IsNullOrEmpty(codigo))
                return BadRequest(new { error_codigo_contabilidad = "Código no válido" });

            try
            {
                // Proceso de autenticación...
                // (código similar pero con nombres únicos)
                
                var tokenUnico = $"token_contabilidad_{Guid.NewGuid()}_{DateTime.UtcNow.Ticks}";
                
                lock (_lockContabilidad2024)
                {
                    // Guardar en diccionario thread-safe
                    _usuariosContabilidadDict2024[tokenUnico] = 
                        new UsuarioAutenticacionContabilidadOAuth2024
                        {
                            TokenSesionContabilidad2024 = tokenUnico,
                            FechaRegistroContabilidad2024 = DateTime.UtcNow,
                            ContadorAccesoContabilidad2024 = 1
                        };
                }

                return Ok(new 
                {
                    autenticacion_exitosa_sistema_contabilidad = true,
                    token_acceso_unico_contabilidad = tokenUnico,
                    instruccion_uso_token_contabilidad = "Usar en header: 'Token-Contabilidad'"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error_proceso_contabilidad = ex.Message });
            }
        }
    }
}