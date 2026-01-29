using Universidad.Entidades;

namespace Universidad.Core.DTOs
{
    public class CartaCreateDTO
    {
        public string EstudianteCI { get; set; }
        public string NITEmpresa { get; set; }
        public string? FolioCarta { get; set; }
        public string Url { get; set; }
    }
    public class CartaReadDto
    {
        public string Codigo { get; set; }
        public string Estudiante { get; set; }
        public string Empresa { get; set; }
        public string? Estado { get; set; }
        public string? Url { get; set; }
    }

    public class CartaUpdateDto
    {
        public string EstudianteCI { get; set; }
        public string NITEmpresa { get; set; }
    }

    public static class CartaMapper
    {
        public static CartaReadDto ToReadDTO(this Carta c)
        {
            return new CartaReadDto
            {
                Codigo = c.Codigo,
                Estudiante = c.EstudianteCI,
                Empresa = c.NITEmpresa,
                Estado = c.Estado,
                Url = c.Url
            };
        }
    }
}