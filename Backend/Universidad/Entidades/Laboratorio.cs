namespace Entidades
{
    public class Laboratorio
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string DocenteResponsable { get; set; } = string.Empty;
        public string Coordinador { get; set; } = string.Empty;
        public string Aula { get; set; } = string.Empty;
        public string Estado { get; set; } = "Disponible";
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
        public bool Activo { get; set; } = true;
    }
}
