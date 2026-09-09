using FireAPI.Data;
using FireAPI.Endpoints;
using FireAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configurar Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. Registrar DbContext con SQL Server
builder.Services.AddDbContext<FireContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Registrar servicios de aplicación
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

// Pipeline de peticiones HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Mapeo de Endpoints
app.MapUsuarioAPI();
app.MapVoluntarioAPI();
app.MapCompaniaAPI();
app.MapCuartelAPI();
app.MapAsistenciaAPI();
app.MapRangoAPI();
app.MapRolAPI();

app.Run();