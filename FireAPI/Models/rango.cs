using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FireAPI.Models
{
    public partial class Rango
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        [JsonIgnore]
        public virtual ICollection<Voluntario> Voluntarios { get; set; } = new List<Voluntario>();
    }
}