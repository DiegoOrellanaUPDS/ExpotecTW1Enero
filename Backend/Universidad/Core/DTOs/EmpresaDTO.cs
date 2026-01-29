using Entidades;
using Universidad.Entidades;

namespace Universidad.Core.DTOs
{
    public class EmpresaCreateDTO
    {
        public string NIT { get; set; }
        public string Nombre { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
    }
    public class EmpresaReadDTO
    {
        public string NIT { get; set; }
        public string Nombre { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
    }
    public class EmpresaUpdateDTO
    {
        public string Nombre { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
    }
    public static class EmpresaMapper
    {
        public static EmpresaReadDTO ToReadDTO(this Empresa e)
        {
            return new EmpresaReadDTO
            {
                NIT = e.NIT,
                Nombre = e.Nombre,
                RazonSocial = e.RazonSocial,
                Direccion = e.Direccion,
                Telefono = e.Telefono,
                Correo = e.Correo
            };
        } 
    }
}