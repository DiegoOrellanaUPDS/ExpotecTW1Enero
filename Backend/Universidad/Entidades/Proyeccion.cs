using System;

namespace Entidades
{
    public class Proyeccion
    {
        public int Id { get; set; }
        public int LlaveForaneaIdMateria { get; set; }
        public int LlaveForaneaIdModulo { get; set; }
        public string Prerrequisito { get; set; }
        public string EstadoProyeccion { get; set; }
        public string Estado { get; set; }
    }
}