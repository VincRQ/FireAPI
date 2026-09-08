using System;

namespace FireAPI.Models
{
    public partial class Asistencia
    {
        public int Id { get; set; }
        public string VoluntarioRut { get; set; } = string.Empty;
        public string DescripcionEmergencia { get; set; } = string.Empty;
        public string TipoEmergencia { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }

        public virtual Voluntario Voluntario { get; set; } = null!;
    }
}