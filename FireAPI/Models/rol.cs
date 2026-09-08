using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FireAPI.Models
{
    public partial class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }

        [JsonIgnore]
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}