using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Universidad.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        // ✅ Inicia OAuth con DISCORD
        // Ej: /api/auth/login?returnUrl=/swagger
        [HttpGet("login")]
        [AllowAnonymous]
        public IActionResult Login([FromQuery] string? returnUrl = "/swagger")
        {
            // Seguridad: solo redirecciones locales
            if (string.IsNullOrWhiteSpace(returnUrl) || !returnUrl.StartsWith("/"))
                returnUrl = "/swagger";

            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(DiscordCallback), new { returnUrl })
            };

            // 👇 IMPORTANTE: esquema DISCORD (NO GitHub)
            return Challenge(props, "Discord");
        }

        // ✅ "Callback" final (después que Discord autentica y crea la cookie)
        [HttpGet("discord/callback")]
        public IActionResult DiscordCallback([FromQuery] string? returnUrl = "/swagger")
        {
            if (User?.Identity?.IsAuthenticated != true)
                return Unauthorized(new { mensaje = "No autenticado" });

            if (string.IsNullOrWhiteSpace(returnUrl) || !returnUrl.StartsWith("/"))
                returnUrl = "/swagger";

            return LocalRedirect(returnUrl);
        }

        // ✅ Ver sesión/claims/tokens (para demostrar gestión de sesión)
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);
            var name = User.FindFirstValue(ClaimTypes.Name) ?? User.Identity?.Name;

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var expiresAt = await HttpContext.GetTokenAsync("expires_at");

            return Ok(new
            {
                userId,
                email,
                name,
                accessTokenPresente = !string.IsNullOrWhiteSpace(accessToken),
                expiresAt
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { mensaje = "Sesión cerrada" });
        }
    }
}
