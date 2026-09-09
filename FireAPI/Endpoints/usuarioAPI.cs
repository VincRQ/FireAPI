using FireAPI.Models;
using FireAPI.Data;
using FireAPI.Services;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FireAPI.Endpoints
{
    public static class UsuarioAPI
    {
        public static void MapUsuarioAPI(this WebApplication app)
        {
            var usuarios = app.MapGroup("/api/usuarios").WithTags("Usuarios");

            // Registro de Usuario
            usuarios.MapPost("/registro", async (RegisterRequest request, FireContext db, AuthService auth) =>
            {
                if (await db.Usuarios.AnyAsync(u => u.Correo == request.Correo))
                {
                    return Results.BadRequest("El correo ya está registrado.");
                }

                var usuario = new Usuario
                {
                    Nombre = request.Nombre,
                    Correo = request.Correo,
                    RolId = request.RolId,
                    VoluntarioRut = request.VoluntarioRut,
                    FechaRegistro = DateTime.Now
                };

                usuario.PasswordHash = auth.HashPassword(usuario, request.Password);

                db.Usuarios.Add(usuario);
                await db.SaveChangesAsync();

                return Results.Created($"/api/usuarios/{usuario.Id}", new { usuario.Id, usuario.Nombre, usuario.Correo });
            });

            // Login
            usuarios.MapPost("/login", async (LoginRequest login, FireContext db, AuthService auth, IConfiguration config) =>
            {
                var usuario = await db.Usuarios
                    .Include(u => u.Rol)
                    .FirstOrDefaultAsync(u => u.Correo == login.Correo);

                if (usuario == null || !auth.VerifyPassword(usuario, usuario.PasswordHash, login.Password))
                {
                    return Results.Unauthorized();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.Nombre),
                    new Claim(ClaimTypes.Email, usuario.Correo),
                    new Claim(ClaimTypes.Role, usuario.Rol.Nombre),
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())
                };

                if (!string.IsNullOrEmpty(usuario.VoluntarioRut))
                {
                    claims.Add(new Claim("VoluntarioRut", usuario.VoluntarioRut));
                }

                var jwtKey = config["Jwt:Key"]!;
                var jwtIssuer = config["Jwt:Issuer"]!;
                var jwtAudience = config["Jwt:Audience"]!;
                var jwtExpireMinutes = config["Jwt:ExpireMinutes"] ?? "120";

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: jwtIssuer,
                    audience: jwtAudience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtExpireMinutes)),
                    signingCredentials: credenciales
                );

                return Results.Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            });

            // Obtener todos los usuarios
            usuarios.MapGet("/", async (FireContext db) =>
            {
                var lista = await db.Usuarios
                    .Include(u => u.Rol)
                    .Select(u => new { u.Id, u.Nombre, u.Correo, u.FechaRegistro, Rol = u.Rol.Nombre, u.VoluntarioRut })
                    .ToListAsync();

                return Results.Ok(lista);
            });

            // Obtener usuario por ID
            usuarios.MapGet("/{id:int}", async (int id, FireContext db) =>
            {
                var usuario = await db.Usuarios
                    .Include(u => u.Rol)
                    .Where(u => u.Id == id)
                    .Select(u => new { u.Id, u.Nombre, u.Correo, u.FechaRegistro, Rol = u.Rol.Nombre, u.VoluntarioRut })
                    .FirstOrDefaultAsync();

                return usuario is not null ? Results.Ok(usuario) : Results.NotFound();
            });
        }

        public record RegisterRequest(string Nombre, string Correo, string Password, int RolId, string? VoluntarioRut);
        public record LoginRequest(string Correo, string Password);
    }
}
