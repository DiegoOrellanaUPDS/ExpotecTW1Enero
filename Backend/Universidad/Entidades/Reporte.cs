using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Universidad.Entidades
{
    public class Reporte
    {
        [Key]
        public int id { get; set; }
        public string codigo { get; set; } 
        public string TipoReporte { get; set; }
        public int IdProyecto { get; set; }
        public string Formato { get; set; } 
        public string UrlArchivo { get; set; } 
        public DateOnly FechaGeneracion { get; set; }
        public string estado { get; set; }
    }
}