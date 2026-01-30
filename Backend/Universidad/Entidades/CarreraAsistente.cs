namespace Universidad.Entidades
{
    public class CarreraAsistente
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Facultad { get; set; }
        public string Descripcion { get; set; }
        public int DuracionSemestres { get; set; }
        public int CreditosTotales { get; set; }
        public bool Activa { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int? JefeCarreraId { get; set; } 
    }
}