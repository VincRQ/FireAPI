using FireAPI.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace FireAPI.Endpoints
{
    public static class VoluntarioAPI
    {
        public static void MapVoluntarioAPI(this WebApplication app)
        {
            var voluntarios = app.MapGroup("/api/voluntarios").WithTags("Voluntarios");

            voluntarios.MapGet("/", async (FireContext db) =>
                await db.Voluntarios
                    .Include(v => v.Rango)
                    .Include(v => v.Compania)
            .ToListAsync());

            voluntarios.MapGet("/{rut}", async (string rut, FireContext db) =>
            {
                var voluntario = await db.Voluntarios
                    .Include(v => v.Rango)
                    .Include(v => v.Compania)
                    .FirstOrDefaultAsync(v => v.Rut == rut);

                return voluntario is not null ? Results.Ok(voluntario) : Results.NotFound();
            });

            voluntarios.MapPost("/", async (Voluntario voluntario, FireContext db) =>
            {
                db.Voluntarios.Add(voluntario);
                await db.SaveChangesAsync();
                return Results.Created($"/api/voluntarios/{voluntario.Rut}", voluntario);
            });

            voluntarios.MapPut("/{rut}", async (string rut, Voluntario inputVoluntario, FireContext db) =>
            {
                var voluntario = await db.Voluntarios.FindAsync(rut);
                if (voluntario is null) return Results.NotFound();

                voluntario.Nombre = inputVoluntario.Nombre;
                voluntario.Apellido = inputVoluntario.Apellido;
                voluntario.RangoId = inputVoluntario.RangoId;
                voluntario.CompaniaId = inputVoluntario.CompaniaId;

                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            voluntarios.MapDelete("/{rut}", async (string rut, FireContext db) =>
            {
                if (await db.Voluntarios.FindAsync(rut) is Voluntario voluntario)
                {
                    db.Voluntarios.Remove(voluntario);
                    await db.SaveChangesAsync();
                    return Results.NoContent();
                }
                return Results.NotFound();
            });
        }
    }
}