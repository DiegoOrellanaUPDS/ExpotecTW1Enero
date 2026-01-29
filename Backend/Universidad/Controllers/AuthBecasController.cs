using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Data;
using Universidad.Entidades;

namespace Universidad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthBecasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public AuthBecasController(AppDbContext context, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
        }

        // 1. ENDPOINT LOGIN - Inicia el flujo OAuth2
        [HttpGet("login")]
        public IActionResult Login()
        {
            var clientId = _configuration["Discord:ClientId"];
            var redirectUri = _configuration["Discord:RedirectUri"];
            var scope = "identify email";

            var authUrl = $"https://discord.com/api/oauth2/authorize?" +
                         $"client_id={clientId}&" +
                         $"redirect_uri={Uri.EscapeDataString(redirectUri)}&" +
                         $"response_type=code&" +
                         $"scope={Uri.EscapeDataString(scope)}";

            return Ok(new { authUrl });
        }

        // 2. ENDPOINT CALLBACK - Procesa la respuesta de Discord
        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery] string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest(new { error = "Código de autorización no recibido" });
            }

            try
            {
                // Paso 1: Intercambiar código por access token
                var tokenResponse = await ExchangeCodeForToken(code);
                
                if (tokenResponse == null)
                {
                    return BadRequest(new { error = "Error al obtener el token de Discord" });
                }

                // Paso 2: Obtener información del usuario de Discord
                var discordUser = await GetDiscordUserInfo(tokenResponse.AccessToken);
                
                if (discordUser == null)
                {
                    return BadRequest(new { error = "Error al obtener información del usuario" });
                }

                // Paso 3: Guardar o actualizar usuario en la base de datos
                var usuario = await SaveOrUpdateUser(discordUser, tokenResponse);

                // Paso 4: Generar JWT token para nuestra aplicación
                var jwtToken = GenerateJwtToken(usuario);

                return Ok(new
                {
                    message = "Autenticación exitosa",
                    token = jwtToken,
                    user = new
                    {
                        id = usuario.Id,
                        discordId = usuario.DiscordId,
                        username = usuario.Username,
                        email = usuario.Email,
                        avatar = usuario.Avatar
                    },
                    expiresAt = usuario.TokenExpiration
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error interno del servidor", details = ex.Message });
            }
        }

        // 3. MÉTODO PRIVADO: Intercambiar código por token
        private async Task<TokenResponse?> ExchangeCodeForToken(string code)
        {
            var clientId = _configuration["Discord:ClientId"];
            var clientSecret = _configuration["Discord:ClientSecret"];
            var redirectUri = _configuration["Discord:RedirectUri"];

            var requestBody = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", redirectUri)
            });

            var response = await _httpClient.PostAsync("https://discord.com/api/oauth2/token", requestBody);
            
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var tokenData = JsonSerializer.Deserialize<JsonElement>(content);

            return new TokenResponse
            {
                AccessToken = tokenData.GetProperty("access_token").GetString(),
                RefreshToken = tokenData.GetProperty("refresh_token").GetString(),
                ExpiresIn = tokenData.GetProperty("expires_in").GetInt32()
            };
        }

        // 4. MÉTODO PRIVADO: Obtener información del usuario de Discord
        private async Task<DiscordUser?> GetDiscordUserInfo(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync("https://discord.com/api/users/@me");
            
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var userData = JsonSerializer.Deserialize<JsonElement>(content);

            return new DiscordUser
            {
                Id = userData.GetProperty("id").GetString(),
                Username = userData.GetProperty("username").GetString(),
                Email = userData.TryGetProperty("email", out var email) ? email.GetString() : null,
                Avatar = userData.TryGetProperty("avatar", out var avatar) ? avatar.GetString() : null
            };
        }

        // 5. MÉTODO PRIVADO: Guardar o actualizar usuario
        private async Task<UsuarioBecasOAuth> SaveOrUpdateUser(DiscordUser discordUser, TokenResponse tokenResponse)
        {
            var usuario = await _context.UsuariosBecasOAuth
                .FirstOrDefaultAsync(u => u.DiscordId == discordUser.Id);

            var tokenExpiration = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn);

            if (usuario == null)
            {
                // Crear nuevo usuario
                usuario = new UsuarioBecasOAuth
                {
                    DiscordId = discordUser.Id,
                    Username = discordUser.Username,
                    Email = discordUser.Email,
                    Avatar = discordUser.Avatar,
                    AccessToken = tokenResponse.AccessToken,
                    RefreshToken = tokenResponse.RefreshToken,
                    TokenExpiration = tokenExpiration,
                    FechaCreacion = DateTime.UtcNow,
                    FechaUltimoAcceso = DateTime.UtcNow
                };
                _context.UsuariosBecasOAuth.Add(usuario);
            }
            else
            {
                // Actualizar usuario existente
                usuario.Username = discordUser.Username;
                usuario.Email = discordUser.Email;
                usuario.Avatar = discordUser.Avatar;
                usuario.AccessToken = tokenResponse.AccessToken;
                usuario.RefreshToken = tokenResponse.RefreshToken;
                usuario.TokenExpiration = tokenExpiration;
                usuario.FechaUltimoAcceso = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return usuario;
        }

        // 6. MÉTODO PRIVADO: Generar JWT Token
        private string GenerateJwtToken(UsuarioBecasOAuth usuario)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.DiscordId),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email ?? ""),
                new Claim("username", usuario.Username),
                new Claim("userId", usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpirationMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // CLASES AUXILIARES
        private class TokenResponse
        {
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
            public int ExpiresIn { get; set; }
        }

        private class DiscordUser
        {
            public string Id { get; set; }
            public string Username { get; set; }
            public string? Email { get; set; }
            public string? Avatar { get; set; }
        }
    }
}