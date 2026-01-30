namespace Universidad.Entidades
{
    public class UsuarioAsistente
    {
        public int Id { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Rol { get; set; } // "Estudiante", "Docente", "JefeCarrera", "Asistente"
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}