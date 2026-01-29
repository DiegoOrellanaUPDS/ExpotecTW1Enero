using System;

namespace Entidades
{
    public class Semestre
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public int Anio { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public string Estado { get; set; }
    }
}