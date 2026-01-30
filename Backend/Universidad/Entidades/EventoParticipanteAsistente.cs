namespace Universidad.Entidades
{
    public class EventoParticipanteAsistente
    {
        public int Id { get; set; }
        public string EstadoAsistencia { get; set; } // "Confirmado", "Asistio", "Falto", "Cancelado"
        public DateTime? FechaConfirmacion { get; set; }
        public string Comentarios { get; set; }
        public int EventoId { get; set; } // Solo ID
        public int UsuarioId { get; set; } // Solo ID
    }
}