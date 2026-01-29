using System;

namespace Entidades
{
    public class Prestamos
    {
        public int id { get; set; }
        public int idlibro { get; set; }
        public int idestudiante { get; set; }
        public DateTime fechaprestamo { get; set; }
        public DateTime fechadevolucion { get; set; }
        public string estado { get; set; }
    }
}