using FireAPI.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace FireAPI.Endpoints
{
    public static class CuartelAPI
    {
        public static void MapCuartelAPI(this WebApplication app)
        {
            var cuarteles = app.MapGroup("/api/cuarteles").WithTags("Cuarteles");

            cuarteles.MapGet("/", async (FireContext db) =>
            await db.Cuarteles.ToListAsync());

            cuarteles.MapGet("/{id:int}", async (int id, FireContext db) =>
                await db.Cuarteles.FindAsync(id) is Cuartel cuartel ? Results.Ok(cuartel) : Results.NotFound());

            cuarteles.MapPost("/", async (Cuartel cuartel, FireContext db) =>
            {
                db.Cuarteles.Add(cuartel);
                await db.SaveChangesAsync();
                return Results.Created($"/api/cuarteles/{cuartel.Id}", cuartel);
            });

            cuarteles.MapPut("/{id:int}", async (int id, Cuartel inputCuartel, FireContext db) =>
            {
                var cuartel = await db.Cuarteles.FindAsync(id);
                if (cuartel is null) return Results.NotFound();

                cuartel.Nombre = inputCuartel.Nombre;
                cuartel.Numero = inputCuartel.Numero;
                cuartel.Direccion = inputCuartel.Direccion;

                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            cuarteles.MapDelete("/{id:int}", async (int id, FireContext db) =>
            {
                if (await db.Cuarteles.FindAsync(id) is Cuartel cuartel)
                {
                    db.Cuarteles.Remove(cuartel);
                    await db.SaveChangesAsync();
                    return Results.NoContent();
                }
                return Results.NotFound();
            });
        }
    }
}