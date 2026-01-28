namespace Universidad.Entidades
{
    public class Usuario_Becas
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string CorreoElectronico { get; set; }
        public string PasswordHash { get; set; } // Para almacenar la contraseña encriptada
        public bool EstaActivo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaUltimoAcceso { get; set; }
    }
}
