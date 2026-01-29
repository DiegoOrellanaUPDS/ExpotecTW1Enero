using Data;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Controllers
{
    [ApiController]
    [Route("oauthth")]
    public class UsuarioTHController : ControllerBase
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly AppDbContext context;
        public const string ClientID="1466290228250673305";
        public const string ClientSecret="Jl0SODfRiuWNupuRw5HgkLi68GYHzTsg";
        public UsuarioTHController(IHttpClientFactory httpClientFactory,AppDbContext context)
        {
            this.httpClientFactory=httpClientFactory;
            this.context=context;
        }
        [HttpGet("usuariosTH")]
        public async Task<IActionResult> GetUsuariosTH()
        {
            return Ok(await (from u in context.UsuarioTHs where u.Estado=="activo" select u).ToListAsync());
        }
        // http://localhost:5024/oauthth/discord-callback
        [HttpGet("login-discord")]
        public async Task<IActionResult> LoginDiscord()
        {
            var uri="http://localhost:5024/oauthth/discord-callback";

            var url="https://discord.com/api/oauth2/authorize" +
          $"?client_id={ClientID}" +
          $"&redirect_uri={uri}" +
          "&response_type=code" + // Discord pide este parámetro extra
          "&scope=identify";  
            return Redirect(url);
        }
        [HttpGet("discord-callback")]
        public async Task<IActionResult> DiscordCallback([FromQuery] string code)
        {
            var redirectionUri="http://localhost:5024/oauthth/discord-callback";

            var client=httpClientFactory.CreateClient();


            //datos en el post de discord requeridos
            var tokenDiscordPost=new Dictionary<string, string>
            {
              ["client_id"]=ClientID,
              ["client_secret"]=ClientSecret,
              ["grant_type"]= "authorization_code",
              ["code"]=code ,
              ["redirect_uri"]=redirectionUri
            };

            var response=await client.PostAsync("https://discord.com/api/oauth2/token",new FormUrlEncodedContent(tokenDiscordPost));

            var body= await response.Content.ReadAsStringAsync();

            var token= body.Split(",").First(x=> x.Contains("\"access_token\"")).Split(":")[1].Replace("\"","").Trim();

            //perdi nombre del usuario
            var request= new HttpRequestMessage(HttpMethod.Get, "https://discord.com/api/users/@me") ;
            request.Headers.Add("Authorization","Bearer "+token);

            var respuestasDis= await client.SendAsync(request);

            var cuerpoDis=await respuestasDis.Content.ReadAsStringAsync();

            var nombreUsuarioDiscord= cuerpoDis.Split(",").First(x=>x.Contains("\"username\"")).Split(":")[1].Replace("\"","").Trim();


            var usuarioLocal=await context.UsuarioTHs.FirstOrDefaultAsync(u=> u.NombreUsuario==nombreUsuarioDiscord);


            if (usuarioLocal==null)
            {
                var nuevoUsuario= new UsuarioTH();
                nuevoUsuario.NombreUsuario=nombreUsuarioDiscord;
                nuevoUsuario.Rol="reclutador";
                nuevoUsuario.Token=token;
                nuevoUsuario.Estado="activo";

                context.UsuarioTHs.Add(nuevoUsuario);
                await context.SaveChangesAsync();
                HttpContext.Session.SetString("UsuarioId",nuevoUsuario.Id.ToString());
            }
            else
            {
                usuarioLocal.Token=token;
                await context.SaveChangesAsync(); 
                HttpContext.Session.SetString("UsuarioId",usuarioLocal.Id.ToString());
            }

            return Ok("Usuario guardado en discord");
        }

    }
}