using FireAPI.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace FireAPI.Endpoints
{
    public static class AsistenciaAPI
    {
        public static void MapAsistenciaAPI(this WebApplication app)
        {
            var asistencias = app.MapGroup("/api/asistencias").WithTags("Asistencias");

            asistencias.MapGet("/", async (FireContext db) =>
                await db.Asistencias.Include(a => a.Voluntario).ToListAsync());

            asistencias.MapGet("/{id:int}", async (int id, FireContext db) =>
                await db.Asistencias.Include(a => a.Voluntario).FirstOrDefaultAsync(a => a.Id == id)
                    is Asistencia asistencia ? Results.Ok(asistencia) : Results.NotFound());

            asistencias.MapGet("/voluntario/{rut}", async (string rut, FireContext db) =>
                await db.Asistencias.Where(a => a.VoluntarioRut == rut).ToListAsync());

            asistencias.MapPost("/", async (Asistencia asistencia, FireContext db) =>
            {
                if (asistencia.FechaHora == default)
                {
                    asistencia.FechaHora = DateTime.Now;
                }

                db.Asistencias.Add(asistencia);
                await db.SaveChangesAsync();
                return Results.Created($"/api/asistencias/{asistencia.Id}", asistencia);
            });

            asistencias.MapDelete("/{id:int}", async (int id, FireContext db) =>
            {
                if (await db.Asistencias.FindAsync(id) is Asistencia asistencia)
                {
                    db.Asistencias.Remove(asistencia);
                    await db.SaveChangesAsync();
                    return Results.NoContent();
                }
                return Results.NotFound();
            });
        }
    }
}