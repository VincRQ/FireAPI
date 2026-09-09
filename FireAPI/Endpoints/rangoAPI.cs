using FireAPI.Models;
using FireAPI.Data;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace FireAPI.Endpoints
{
    public static class RangoAPI
    {
        public static void MapRangoAPI(this WebApplication app)
        {
            var rangos = app.MapGroup("/api/rangos").WithTags("Rangos");

            rangos.MapGet("/", async (FireContext db) => await db.Rangos.ToListAsync());

            rangos.MapGet("/{id:int}", async (int id, FireContext db) =>
                await db.Rangos.FindAsync(id) is Rango rango ? Results.Ok(rango) : Results.NotFound());
        }
    }
}