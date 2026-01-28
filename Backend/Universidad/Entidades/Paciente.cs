using System;
namespace Entidades
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string CI { get; set; }
        public string Sexo { get; set; }
        public string TipoSangre { get; set; }
        public string Estado { get; set; }
    }
}
