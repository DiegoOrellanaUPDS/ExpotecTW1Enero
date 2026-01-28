namespace Universidad.Entidades
{
    public class SolicitudBeca
    {
        public int Id { get; set; }
        public int EstudianteId { get; set; } 
        public int BecaId { get; set; }
        public string Estado { get; set; } 
        public string Justificacion { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaRespuesta { get; set; }
        public string Observaciones { get; set; } 
        public int? RevisadoPor { get; set; } 
    }
}
