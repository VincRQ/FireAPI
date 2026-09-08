using System;
using System.Text.Json.Serialization;

namespace FireAPI.Models
{
    public partial class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;

        [JsonIgnore]
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public int RolId { get; set; }
        public string? VoluntarioRut { get; set; }

        public virtual Rol Rol { get; set; } = null!;
        public virtual Voluntario? Voluntario { get; set; }
    }
}