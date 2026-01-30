namespace Universidad.Entidades
{
    public class SolicitudAsistente
    {
        public int Id { get; set; }
        public string Tipo { get; set; } // "CambioMateria", "PermisoEspecial", "RevisionNota", "BajaTemporal"
        public string Descripcion { get; set; }
        public string Estado { get; set; } // "Pendiente", "Aprobada", "Rechazada", "EnRevision"
        public string MotivoRechazo { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaRevision { get; set; }
        public int EstudianteId { get; set; } // Solo ID
        public int? RevisadoPorId { get; set; } // Solo ID
    }
}