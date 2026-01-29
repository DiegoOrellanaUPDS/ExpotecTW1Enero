namespace Universidad.Entidades
{
    public class Contrato
    {
        public int Id { get; set; }

        public int EmpleadoId { get; set; }

        public string TipoContrato { get; set; } = string.Empty;

        public decimal Salario { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }
    }
}
