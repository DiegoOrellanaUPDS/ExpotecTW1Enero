namespace Universidad.Entidades
{
    public class ReporteAsistente
    {
        public int Id { get; set; }
        public string Tipo { get; set; } // "RendimientoAcademico", "Asistencia", "Desercion", "Satisfaccion"
        public string Periodo { get; set; } // "2024-1", "2024-2"
        public decimal PromedioGeneral { get; set; }
        public decimal PorcentajeDesercion { get; set; }
        public int TotalEstudiantes { get; set; }
        public int EstudiantesAprobados { get; set; }
        public int EstudiantesReprobados { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public int GeneradoPorId { get; set; } // Solo ID
        public int? CarreraId { get; set; } // Solo ID
    }
}