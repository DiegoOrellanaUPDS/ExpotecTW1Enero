using System;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class ContabilidadReportesIngresos
    {
        [Key]
        public int IdReporteContabilidad { get; set; }
        public decimal TotalIngresoContabilidad { get; set; }
        public decimal TotalEgresosContabilidad { get; set; }
        public decimal BalanceContabilidad { get; set; }
        public DateTime FechaContabilidad { get; set; }
    }
}