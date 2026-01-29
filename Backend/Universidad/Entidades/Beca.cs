namespace Universidad.Entidades
{
    public class Beca
    {
        public int Id { get; set; }
        public string NombreBeca { get; set; }
        public string Descripcion { get; set; }
        public string TipoBeca { get; set; } 
        public decimal MontoCobertura { get; set; } 
        public string Requisitos { get; set; }
        public int DuracionMeses { get; set; }
        public bool EstaActiva { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
