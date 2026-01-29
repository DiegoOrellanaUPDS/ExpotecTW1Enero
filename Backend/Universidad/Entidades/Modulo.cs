using System;

namespace Entidades
{
    public class Modulo
    {
        public int Id { get; set; }
        public int NumeroModulo { get; set; }
        public string Codigo { get; set; }

        public int Anio { get; set; }
        public int Mes { get; set; }
        public string Turno { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public int LlaveForaneaIdMateria { get; set; }
        public int LlaveForaneaIdSemestre { get; set; }
        public string Estado { get; set; }
    }
}