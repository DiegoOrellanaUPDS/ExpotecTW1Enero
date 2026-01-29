using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class PracticaProfesional
    {
        [Key]
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string EstudianteCI { get; set; }
        public string NITEmpresa { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public int HorasRequeridas { get; set; }
        public string AreaDepartamento { get; set; } //en donde trabajara el practicante
        public string Estado { get; set; }
        public DateOnly FechaRegistro { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}