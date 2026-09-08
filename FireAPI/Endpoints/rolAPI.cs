using FireAPI.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace FireAPI.Endpoints
{
    public static class RolAPI
    {
        public static void MapRolAPI(this WebApplication app)
        {
            var roles = app.MapGroup("/api/roles").WithTags("Roles");

            roles.MapGet("/", async (FireContext db) => await db.Roles.ToListAsync());

            roles.MapGet("/{id:int}", async (int id, FireContext db) =>
                await db.Roles.FindAsync(id) is Rol rol ? Results.Ok(rol) : Results.NotFound());
        }
    }
}