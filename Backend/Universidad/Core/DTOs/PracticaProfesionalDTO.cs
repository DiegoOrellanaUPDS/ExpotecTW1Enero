using Entidades;
using Universidad.Entidades;

namespace Universidad.Core.DTOs
{
    public class PracticaProfesionalCreateDTO
    {
        public string EstudianteCI { get; set; }
        public string NITEmpresa { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public int HorasRequeridas { get; set; }
        public string AreaDepartamento { get; set; }
    }
    public class PracticaProfesionalReadDTO
    {
        public string Codigo { get; set; }
        public string EstudianteCI { get; set; }
        public string NITEmpresa { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public int HorasRequeridas { get; set; }
        public string AreaDepartamento { get; set; }
    }
    public class PracticaProfesionalUpdateDTO
    {
        public string EstudianteCI { get; set; }
        public string NITEmpresa { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public int HorasRequeridas { get; set; }
        public string AreaDepartamento { get; set; }
    }
    public static class PracticaProfesionalMapper
    {
        public static PracticaProfesionalReadDTO ToReadDTO(this PracticaProfesional pp)
        {
            return new PracticaProfesionalReadDTO
            {
                Codigo = pp.Codigo,
                EstudianteCI = pp.EstudianteCI,
                NITEmpresa = pp.NITEmpresa,
                FechaInicio = pp.FechaInicio,
                FechaFin = pp.FechaFin,
                HorasRequeridas = pp.HorasRequeridas,
                AreaDepartamento = pp.AreaDepartamento
            };
        }
    }
}