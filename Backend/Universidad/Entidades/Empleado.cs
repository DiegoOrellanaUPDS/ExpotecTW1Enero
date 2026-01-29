namespace Universidad.Entidades
{
    public class Empleado
    {
        public int Id { get; set; }

        public int PersonaId { get; set; }

        public string CodigoEmpleado { get; set; } = string.Empty;

        public DateTime FechaIngreso { get; set; }

        public bool Activo { get; set; }
    }
}
