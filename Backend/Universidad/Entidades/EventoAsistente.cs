namespace Universidad.Entidades
{
    public class EventoAsistente
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; } // "Conferencia", "Taller", "Reunion", "Examen"
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Ubicacion { get; set; }
        public int CupoMaximo { get; set; }
        public bool RequiereConfirmacion { get; set; }
        public string Estado { get; set; } // "Programado", "EnCurso", "Finalizado", "Cancelado"
        public DateTime FechaCreacion { get; set; }
        public int OrganizadorId { get; set; } // Solo ID
    }
}