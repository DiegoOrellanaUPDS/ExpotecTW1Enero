using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Universidad.Entidades
{
    public class Proyecto
    {
        [Key]
        public int id { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public string colaboradores { get; set; }
        public DateOnly fechaInicio { get; set; }
        public DateOnly fechaFin { get; set; }
        public string codigo { get; set; }
        public string estado { get; set; }
    }
}