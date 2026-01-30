namespace Universidad.Entidades
{
    public class MateriaAsistente
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Creditos { get; set; }
        public int Semestre { get; set; }
        public string Tipo { get; set; } 
        public bool Activa { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int CarreraId { get; set; } 
        public int? DocenteId { get; set; } 
    }
}