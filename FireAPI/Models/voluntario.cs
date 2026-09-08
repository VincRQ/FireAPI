using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FireAPI.Models
{
    public partial class Voluntario
    {
        public string Rut { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public int RangoId { get; set; }
        public int CompaniaId { get; set; }

        public virtual Rango Rango { get; set; } = null!;
        public virtual Compania Compania { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<Asistencia> Asistencias { get; set; } = new List<Asistencia>();

        [JsonIgnore]
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}