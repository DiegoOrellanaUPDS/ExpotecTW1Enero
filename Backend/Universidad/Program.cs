using Data;
using Microsoft.EntityFrameworkCore;
using Universidad.Services;

var builder = WebApplication.CreateBuilder(args);

// =====================
// Cadena de conexión
// =====================
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
                       ?? builder.Configuration.GetConnectionString("PostgreSQL")
                       ?? "Host=localhost;Database=universidad;Username=postgres;Password=Control123+;Port=5432";

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
        Console.WriteLine("Migraciones aplicadas correctamente.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error aplicando migraciones: " + ex.Message);
    }
}

// =====================
// Middleware
// =====================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Universidad v1");
        c.RoutePrefix = string.Empty; // Swagger en la raíz
    });
}

app.UseCors("MyApp");
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseSession();
app.MapControllers();

app.Run();