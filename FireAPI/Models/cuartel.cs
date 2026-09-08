using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FireAPI.Models
{
    public partial class Cuartel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Numero { get; set; }
        public string? Direccion { get; set; }

        [JsonIgnore]
        public virtual ICollection<Compania> Companias { get; set; } = new List<Compania>();
    }
}