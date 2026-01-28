namespace Entidades
{
    public class ActividadDepa
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public string Encargado { get; set; } = string.Empty;

        public bool Estado { get; set; } = true;

        // Este es tu ID Departamento (Clave foránea)
        public int DepartamentoId { get; set; }
    }
}