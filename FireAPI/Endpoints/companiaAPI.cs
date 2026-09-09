using FireAPI.Models;
using FireAPI.Data;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace FireAPI.Endpoints
{
    public static class CompaniaAPI
    {
        public static void MapCompaniaAPI(this WebApplication app)
        {
            var companias = app.MapGroup("/api/companias").WithTags("Compañías");

            companias.MapGet("/", async (FireContext db) =>
                await db.Companias.Include(c => c.Cuartel).ToListAsync());

            companias.MapGet("/{id:int}", async (int id, FireContext db) =>
                await db.Companias.Include(c => c.Cuartel).FirstOrDefaultAsync(c => c.Id == id)
                    is Compania compania ? Results.Ok(compania) : Results.NotFound());

            companias.MapPost("/", async (Compania compania, FireContext db) =>
            {
                db.Companias.Add(compania);
                await db.SaveChangesAsync();
                return Results.Created($"/api/companias/{compania.Id}", compania);
            });

            companias.MapPut("/{id:int}", async (int id, Compania inputCompania, FireContext db) =>
            {
                var compania = await db.Companias.FindAsync(id);
                if (compania is null) return Results.NotFound();

                compania.Nombre = inputCompania.Nombre;
                compania.Numero = inputCompania.Numero;
                compania.CantidadVoluntarios = inputCompania.CantidadVoluntarios;
                compania.Especialidad = inputCompania.Especialidad;
                compania.CuartelId = inputCompania.CuartelId;

                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            companias.MapDelete("/{id:int}", async (int id, FireContext db) =>
            {
                if (await db.Companias.FindAsync(id) is Compania compania)
                {
                    db.Companias.Remove(compania);
                    await db.SaveChangesAsync();
                    return Results.NoContent();
                }
                return Results.NotFound();
            });
        }
    }
}