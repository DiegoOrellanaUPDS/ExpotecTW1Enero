using Microsoft.EntityFrameworkCore;
using Universidad.Data;

var builder = WebApplication.CreateBuilder(args);

// ====================
// Servicios
// ====================

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Connection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

// ====================
// App
// ====================

var app = builder.Build();

// ====================
// Middleware
// ====================

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

