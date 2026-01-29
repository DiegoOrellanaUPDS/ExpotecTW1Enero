using Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Obtener la cadena de conexión desde las variables de entorno
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
                       ?? builder.Configuration.GetConnectionString("Connection");

// Configurar DbContext con Npgsql
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure();
    }));

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyApp", policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin();
        policyBuilder.AllowAnyHeader();
        policyBuilder.AllowAnyMethod();
    });
});

// =======================
// ✅ OAuth2 DISCORD
// =======================
var discordClientId =
    Environment.GetEnvironmentVariable("DISCORD_CLIENT_ID")
    ?? builder.Configuration["OAuth:Discord:ClientId"];

var discordClientSecret =
    Environment.GetEnvironmentVariable("DISCORD_CLIENT_SECRET")
    ?? builder.Configuration["OAuth:Discord:ClientSecret"];

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = "Discord";
    })
    .AddCookie(options =>
    {
        options.Cookie.Name = "universidad_auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;

        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;

        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = ctx =>
            {
                ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = ctx =>
            {
                ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            }
        };
    })
    .AddDiscord("Discord", options =>
    {
        options.ClientId = discordClientId!;
        options.ClientSecret = discordClientSecret!;

        // ✅ ESTE es el callback del middleware OAuth (no tu controller)
        options.CallbackPath = "/signin-discord";

        options.Scope.Add("identify");
        options.Scope.Add("email");

        options.SaveTokens = true;
    });

builder.Services.AddAuthorization();

// Controllers, Swagger y HttpClient
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

var app = builder.Build();

// Migraciones
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error aplicando migraciones: " + ex.Message);
    }
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
});

app.UseCors("MyApp");

// Si estás en puro HTTP, esto solo da warning (opcional comentarlo)
// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
