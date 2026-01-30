using Data;
using Microsoft.EntityFrameworkCore;
using Universidad.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =====================
// Cadena de conexión PostgreSQL
// =====================
var connectionString = builder.Configuration.GetConnectionString("PostgreSQL") 
                       ?? "Host=localhost;Database=bienestarest;Username=postgres;Password=Control123+;Port=5432";

// =====================
// Servicios
// =====================
builder.Services.AddSingleton<CloudinaryService>(); 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure();
    }));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
<<<<<<< HEAD
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Directamente tu clave del appsettings
        var key = Encoding.UTF8.GetBytes("MiClaveSecretaParaBecasUPDS2025SistemaUniversitario123456");
        
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = "UniversidadUPDS",
            ValidateAudience = true,
            ValidAudience = "BecasModule",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });



builder.Services.AddAuthorization();

builder.Services.AddSession();
=======
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
>>>>>>> ae1d59e (ricardo Valencia Bienestar Estudiantil)
builder.Services.AddDistributedMemoryCache();

// =====================
// App
// =====================
var app = builder.Build();

// =====================
// Migraciones automáticas
// =====================
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        dbContext.Database.Migrate();
        Console.WriteLine("✅ Migraciones de PostgreSQL aplicadas correctamente.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error aplicando migraciones: {ex.Message}");
    }
}

// =====================
// Middleware
// =====================
if (app.Environment.IsDevelopment())
{
<<<<<<< HEAD
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    // Esto hace que Swagger esté en la raíz
    //c.RoutePrefix = string.Empty;
};
app.UseCors("MyApp");
app.UseAuthentication();
=======
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Bienestar Estudiantil v1");
        c.RoutePrefix = string.Empty; // Swagger en la raíz
    });


app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseSession();
app.MapControllers();

app.Run();