using FireAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FireAPI.Data
{
    public class FireContext : DbContext
    {
        public FireContext(DbContextOptions<FireContext> options) : base(options) { }

        public DbSet<Cuartel> Cuarteles { get; set; }
        public DbSet<Compania> Companias { get; set; }
        public DbSet<Rango> Rangos { get; set; }
        public DbSet<Voluntario> Voluntarios { get; set; }
        public DbSet<Asistencia> Asistencias { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
    }
}