namespace Universidad.Entidades
{
    public class BeneficioBeca
    {
        public int Id { get; set; }
        public int BecaId { get; set; } // Relación con Beca
        public string TipoBeneficio { get; set; } // Matrícula, Mensualidad, Material, Transporte, etc.
        public decimal MontoCobertura { get; set; }
        public string Descripcion { get; set; }
        public bool EstaActivo { get; set; }
    }
}
