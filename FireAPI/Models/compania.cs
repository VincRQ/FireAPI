using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FireAPI.Models
{
    public partial class Compania
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Numero { get; set; }
        public int CantidadVoluntarios { get; set; }
        public string Especialidad { get; set; } = string.Empty;
        public int CuartelId { get; set; }

        public virtual Cuartel Cuartel { get; set; } = null!;

        [JsonIgnore]
        public virtual ICollection<Voluntario> Voluntarios { get; set; } = new List<Voluntario>();
    }
}