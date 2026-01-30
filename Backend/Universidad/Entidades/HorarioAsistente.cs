namespace Universidad.Entidades
{
    public class HorarioAsistente
    {
        public int Id { get; set; }
        public string CodigoMateria { get; set; }
        public string NombreMateria { get; set; }
        public string Aula { get; set; }
        public string Seccion { get; set; }
        public int Cupos { get; set; }
        public int CuposDisponibles { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int DocenteId { get; set; } 
        public int MateriaId { get; set; } 
    }
}