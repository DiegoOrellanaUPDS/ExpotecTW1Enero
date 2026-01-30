using Data;
using Microsoft.EntityFrameworkCore;
using Universidad.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// =====================
// Cadena de conexión
// =====================
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
                       ?? builder.Configuration.GetConnectionString("Connection");

// =====================
// Servicios
// =====================

builder.Services.AddSingleton<CloudinaryService>(); 

//builder.Services.AddSingleton<CloudinaryService>(); 

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure();
    }));

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyApp", policy =>
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
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error aplicando migraciones: " + ex.Message);
    }
}

// =====================
// Middleware
// =====================
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    // Esto hace que Swagger esté en la raíz
    //c.RoutePrefix = string.Empty;
});
app.UseCors("MyApp");
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseSession();
app.MapControllers();

app.Run();
