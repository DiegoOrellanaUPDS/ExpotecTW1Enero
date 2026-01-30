namespace Universidad.Entidades
{
    public class HorarioDiaAsistente
    {
        public int Id { get; set; }
        public string DiaSemana { get; set; } 
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public int HorarioId { get; set; } 
    }
}