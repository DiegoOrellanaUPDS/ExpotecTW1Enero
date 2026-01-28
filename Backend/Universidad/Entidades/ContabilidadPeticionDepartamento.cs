using System;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class ContabilidadPeticionDepartamento
    {
        [Key]
        public int IdDepartamentoContabilidad { get; set; }
        public string NombreEncargadoContabilidad { get; set; } = string.Empty;
        public decimal PresupuestoContabilidad { get; set; }
        public DateTime FechaContabilidad { get; set; }
        public string EstadoContabilidad { get; set; } = "Pendiente";
    }
}